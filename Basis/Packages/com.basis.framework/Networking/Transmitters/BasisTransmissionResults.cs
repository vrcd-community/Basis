using Basis.Network.Core;
using Basis.Scripts.BasisSdk;
using Basis.Scripts.BasisSdk.Players;
using Basis.Scripts.Networking;
using Basis.Scripts.Networking.NetworkedAvatar;
using Basis.Scripts.Networking.Receivers;
using Basis.Scripts.Networking.Transmitters;
using Basis.Scripts.Profiler;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using static SerializableBasis;

[System.Serializable]
public partial class BasisTransmissionResults
{
    public List<ushort> TalkingPoints = new List<ushort>(128);

    public float intervalSeconds = 0.05f;
    public float timer = 0f;

    public float SquaredSmallestDistance;
    public float UnClampedInterval;
    public float DefaultInterval;

    [SerializeReference]
    public BasisNetworkTransmitter BasisNetworkTransmitter;

    public NetDataWriter VRMWriter = new NetDataWriter(true, 0);
    public VoiceReceiversMessage VRM = new VoiceReceiversMessage();

    // ---------------------------
    // Change-detection cache
    // ---------------------------
    private int _lastUsersCount = -1;
    private ulong _lastUsersHash = 0;

    // ---------------------------
    // Job-backed buffers + handle
    // ---------------------------
    private NativeArray<ushort> _playerIds;
    private NativeArray<byte> _inMicRange;     // 0/1
    private NativeArray<ushort> _outUsers;     // compacted recipients
    private NativeArray<int> _outCount;        // length 1
    private NativeArray<ulong> _outHash;       // length 1

    private JobHandle _recipientsJobHandle;
    private bool _recipientsJobScheduled;

    // Optional: avoid allocations for VRM.Users
    private ushort[] _usersManagedBuffer;

    // ---------------------------
    // Called each frame
    // ---------------------------
    public void Simulate()
    {
        if (!RemoteBoneJobSystem.sInitialized)
            return;

        float deltaTime = Time.deltaTime;
        timer += deltaTime;

        if (timer <= intervalSeconds)
            return;

        float previousInterval = intervalSeconds;

        if (!TryGetLocalAvatar(previousInterval, out BasisAvatar avatar))
            return;

        int receiverCount = BasisNetworkPlayers.ReceiverCount;
        var snapshot = BasisNetworkPlayers.ReceiversSnapshot;

        BasisNetworkAvatarCompressor.Compress(BasisNetworkTransmitter, avatar.Animator);

        if (!TryRefreshDistanceAndChangeFlags())
            return;

        ComputeAudioRecipientsJob(snapshot, receiverCount);
        UpdateHearing(snapshot, receiverCount);
        UpdateAvatarRanges(snapshot, receiverCount);
        UpdateMeshLods(snapshot, receiverCount);

        RecalculateInterval();

        if (BasisAvatarRecorder.IsRecording)
        {
            StoreRecordingFrame(avatar);
        }

        CompleteAudioRecipientsJobAndMaybeSend();
        timer -= previousInterval;
    }

    // ---------------------------
    // Lifecycle helpers (call from owner MonoBehaviour)
    // ---------------------------

    /// <summary>Call once (e.g., OnEnable) to allocate native buffers.</summary>
    public void InitializeJobBuffers(int initialCapacity = 256)
    {
        initialCapacity = Mathf.Max(1, initialCapacity);

        DisposeJobBuffers(); // in case it was already initialized

        _playerIds = new NativeArray<ushort>(initialCapacity, Allocator.Persistent);
        _inMicRange = new NativeArray<byte>(initialCapacity, Allocator.Persistent);
        _outUsers = new NativeArray<ushort>(initialCapacity, Allocator.Persistent);

        _outCount = new NativeArray<int>(1, Allocator.Persistent);
        _outHash = new NativeArray<ulong>(1, Allocator.Persistent);

        _recipientsJobScheduled = false;
    }

    /// <summary>Call once (e.g., OnDisable/OnDestroy) to dispose native buffers.</summary>
    public void DisposeJobBuffers()
    {
        if (_recipientsJobScheduled)
        {
            _recipientsJobHandle.Complete();
            _recipientsJobScheduled = false;
        }

        if (_playerIds.IsCreated) _playerIds.Dispose();
        if (_inMicRange.IsCreated) _inMicRange.Dispose();
        if (_outUsers.IsCreated) _outUsers.Dispose();
        if (_outCount.IsCreated) _outCount.Dispose();
        if (_outHash.IsCreated) _outHash.Dispose();
    }

    // ---------------------------
    // The "compute" + "complete" methods you asked for
    // ---------------------------

    /// <summary>
    /// Compute(): fills NativeArrays and schedules a Burst job that compacts recipients + computes a signature.
    /// This does NOT send; call CompleteAudioRecipientsJobAndMaybeSend() after.
    /// </summary>
    public void ComputeAudioRecipientsJob(IReadOnlyList<BasisNetworkReceiver> snapshot, int receiverCount)
    {
        EnsureCapacity(receiverCount);

        // Fill the inputs for the job on main thread
        for (int Index = 0; Index < receiverCount; Index++)
        {
            var remote = snapshot[Index];

            _playerIds[Index] = remote.playerId;

            if (!RemoteBoneJobSystem.TryGetMicrophoneRange(remote.playerId, out bool hasMicrophoneRange))
            {
                // treat failures as out of range; also log (optional)
                BasisDebug.LogError("Can't get microphone range!");
                _inMicRange[Index] = 0;
            }
            else
            {
                _inMicRange[Index] = hasMicrophoneRange ? (byte)1 : (byte)0;
            }
        }

        // Schedule the Burst job
        var job = new BasisBuildRecipientsAndSignatureJob
        {
            playerIds = _playerIds,
            inMicRange = _inMicRange,
            length = receiverCount,

            outUsers = _outUsers,
            outCount = _outCount,
            outHash = _outHash
        };

        _recipientsJobHandle = job.Schedule();
        _recipientsJobScheduled = true;
    }

    /// <summary>
    /// Complete(): completes the scheduled job and sends the VRM.Users message only if the set changed.
    /// </summary>
    public void CompleteAudioRecipientsJobAndMaybeSend()
    {
        if (!_recipientsJobScheduled)
            return;

        _recipientsJobHandle.Complete();
        _recipientsJobScheduled = false;

        int newCount = _outCount[0];
        ulong newHash = _outHash[0];

        BasisNetworkTransmitter.HasReasonToSendAudio = newCount != 0;

        // Change detection: no change => don't allocate/serialize/send
        if (newCount == _lastUsersCount && newHash == _lastUsersHash)
            return;

        _lastUsersCount = newCount;
        _lastUsersHash = newHash;

        // Copy recipients into managed buffer for your message type
        EnsureManagedUsersBuffer(newCount);
        for (int i = 0; i < newCount; i++)
            _usersManagedBuffer[i] = _outUsers[i];

        // If VoiceReceiversMessage expects exact-length array, make a trimmed one.
        // To avoid allocations, you can modify VoiceReceiversMessage to serialize count+buffer.
        // Here we allocate only when changed (still much better than every tick).
        var exact = new ushort[newCount];
        Array.Copy(_usersManagedBuffer, exact, newCount);

        VRM.Users = exact;

        VRMWriter.Reset();
        VRM.Serialize(VRMWriter);

        BasisNetworkConnection.LocalPlayerPeer.Send(
            VRMWriter,
            BasisNetworkCommons.AudioRecipientsChannel,
            DeliveryMethod.ReliableOrdered
        );

        BasisNetworkProfiler.AddToCounter(BasisNetworkProfilerCounter.AudioRecipients, VRMWriter.Length);
    }

    // ---------------------------
    // Internal helpers
    // ---------------------------

    private void EnsureCapacity(int receiverCount)
    {
        // If user didn't call InitializeJobBuffers, do a safe default init
        if (!_playerIds.IsCreated)
            InitializeJobBuffers(Mathf.Max(256, receiverCount));

        if (receiverCount <= _playerIds.Length)
            return;

        int newCap = NextPow2(receiverCount);

        // Complete any pending work before resizing
        if (_recipientsJobScheduled)
        {
            _recipientsJobHandle.Complete();
            _recipientsJobScheduled = false;
        }

        // Reallocate
        Realloc(ref _playerIds, newCap);
        Realloc(ref _inMicRange, newCap);
        Realloc(ref _outUsers, newCap);

        // _outCount/_outHash are size 1 and don't need resizing
    }

    private void EnsureManagedUsersBuffer(int count)
    {
        if (_usersManagedBuffer == null || _usersManagedBuffer.Length < count)
            _usersManagedBuffer = new ushort[NextPow2(Mathf.Max(1, count))];
    }

    private static int NextPow2(int v)
    {
        v = Mathf.Max(1, v);
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;
        return v;
    }

    private static void Realloc<T>(ref NativeArray<T> arr, int newLen) where T : struct
    {
        var newArr = new NativeArray<T>(newLen, Allocator.Persistent);
        if (arr.IsCreated)
        {
            NativeArray<T>.Copy(arr, newArr, arr.Length);
            arr.Dispose();
        }
        arr = newArr;
    }
    private bool TryGetLocalAvatar(float previousInterval, out BasisAvatar basisAvatar)
    {
        var player = BasisNetworkTransmitter.Player;
        basisAvatar = player != null ? player.BasisAvatar : null;

        if (basisAvatar == null)
        {
            BasisDebug.LogError("Missing Basis Avatar. Cannot send network update.", BasisDebug.LogTag.System);
            timer -= previousInterval;
            return false;
        }

        return true;
    }

    private bool TryRefreshDistanceAndChangeFlags()
    {
        bool worked = RemoteBoneJobSystem.TryGetSmallestDistanceSq(out SquaredSmallestDistance);
        if (!worked)
        {
            BasisDebug.LogError("Unable to get squared smallest distance!");
            return false;
        }

        if (!float.IsFinite(SquaredSmallestDistance))
        {
            SquaredSmallestDistance = 0f;
        }

        return true;
    }

    private void UpdateHearing(IReadOnlyList<BasisNetworkReceiver> snapshot, int receiverCount)
    {
        for (int index = 0; index < receiverCount; index++)
        {
            var receiver = snapshot[index];

            if (!RemoteBoneJobSystem.TryGetHearingRange(receiver.playerId, out bool canHear))
            {
                BasisDebug.LogError("Can't get hearing range!");
                continue;
            }

            if (receiver.AudioReceiverModule.HasAudioSource == canHear)
            {
                continue;
            }

            if (canHear)
            {
                receiver.AudioReceiverModule.StartAudio();
                receiver.RemotePlayer.OutOfRangeFromLocal = false;
            }
            else
            {
                receiver.AudioReceiverModule.StopAudio();
                receiver.RemotePlayer.OutOfRangeFromLocal = true;
            }
        }
    }

    private void UpdateAvatarRanges(IReadOnlyList<BasisNetworkReceiver> snapshot, int receiverCount)
    {
        for (int index = 0; index < receiverCount; index++)
        {
            var receiver = snapshot[index];
            var remote = receiver.RemotePlayer;

            if (remote.IsLoadingAnAvatar)
                continue;

            if (!RemoteBoneJobSystem.TryGetAvatarRange(receiver.playerId, out bool hasAvatar))
            {
                BasisDebug.LogError("Can't get avatar range!");
                continue;
            }

            if (remote.InAvatarRange == hasAvatar)
                continue;

            remote.InAvatarRange = hasAvatar;
            remote.ReloadAvatar();
        }
    }

    private void UpdateMeshLods(IReadOnlyList<BasisNetworkReceiver> snapshot, int receiverCount)
    {
        float meshLodMultiplier = SMModuleDistanceBasedReductions.MeshLod;

        for (int index = 0; index < receiverCount; index++)
        {
            var receiver = snapshot[index];
            var remote = receiver.RemotePlayer;

            if (!RemoteBoneJobSystem.TryGetDistanceSq(receiver.playerId, out float distanceSq))
            {
                BasisDebug.LogError("Can't get distance sq!");
                continue;
            }

            remote.ChangeMeshLOD(distanceSq, meshLodMultiplier);
        }
    }

    private void RecalculateInterval()
    {
        ServerMetaDataMessage meta = BasisNetworkManagement.ServerMetaDataMessage;

        DefaultInterval = meta.SyncInterval / 1000f;

        float calculatedIntervalBase = meta.BaseMultiplier + (SquaredSmallestDistance * meta.IncreaseRate);
        UnClampedInterval = DefaultInterval * calculatedIntervalBase;

        intervalSeconds = Mathf.Clamp(UnClampedInterval, DefaultInterval, meta.SlowestSendRate);
    }

    private void StoreRecordingFrame(BasisAvatar avatar)
    {
        var anim = avatar.Animator;
        BasisAvatarRecorder.StoreData(intervalSeconds, anim.bodyRotation, anim.bodyPosition, BasisNetworkTransmitter.HumanPose.muscles, anim.transform.localScale.y);
    }
}
