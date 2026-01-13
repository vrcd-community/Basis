using Basis.Network.Core;
using Basis.Scripts.BasisSdk;
using Basis.Scripts.BasisSdk.Players;
using Basis.Scripts.Networking;
using Basis.Scripts.Networking.NetworkedAvatar;
using Basis.Scripts.Networking.Receivers;
using Basis.Scripts.Networking.Transmitters;
using Basis.Scripts.Profiler;
using System.Collections.Generic;
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

    /// <summary>
    /// Called each frame; drives scheduling of distance job and network sync.
    /// </summary>
    public void Simulate()
    {
        if (!RemoteBoneJobSystem.sInitialized)
            return;

        float deltaTime = Time.deltaTime;
        timer += deltaTime;

        if (timer <= intervalSeconds)
            return;

        // Use the actual accumulated interval (handles overshoot)
        float previousInterval = intervalSeconds;

        if (!TryGetLocalAvatar(previousInterval, out BasisAvatar avatar))
        {
            return;
        }

        int receiverCount = BasisNetworkPlayers.ReceiverCount;
        var snapshot = BasisNetworkPlayers.ReceiversSnapshot;
        // Compress avatar state (doesn't touch mouth bone used as input)
        BasisNetworkAvatarCompressor.Compress(BasisNetworkTransmitter, avatar.Animator);

        if (!TryRefreshDistanceAndChangeFlags())
        {
            return;
        }

        UpdateHearing(snapshot, receiverCount);

        UpdateAvatarRanges(snapshot, receiverCount);

        UpdateMeshLods(snapshot, receiverCount);

        SendTalkingPoints(snapshot, receiverCount);

        RecalculateInterval();

        if (BasisAvatarRecorder.IsRecording)
        {
            StoreRecordingFrame(avatar);
        }

        // account for overshoot using the interval that actually accumulated
        timer -= previousInterval;
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
            // No receivers or all failed positions => treat as zero for rate calc
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
                continue;

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

            // Distance-based mesh LOD
            remote.ChangeMeshLOD(distanceSq, meshLodMultiplier);
        }
    }

    private void SendTalkingPoints(IReadOnlyList<BasisNetworkReceiver> snapshot, int receiverCount)
    {
        if (TalkingPoints.Capacity < receiverCount)
        {
            TalkingPoints.Capacity = receiverCount;
        }

        TalkingPoints.Clear();

        for (int index = 0; index < receiverCount; index++)
        {
            var remote = snapshot[index];

            if (!RemoteBoneJobSystem.TryGetMicrophoneRange(remote.playerId, out bool hasMicrophoneRange))
            {
                BasisDebug.LogError("Can't get microphone range!");
                continue;
            }

            if (hasMicrophoneRange)
                TalkingPoints.Add(remote.playerId);
        }

        BasisNetworkTransmitter.HasReasonToSendAudio = TalkingPoints.Count != 0;

        VRM.Users = TalkingPoints.ToArray();

        VRMWriter.Reset();
        VRM.Serialize(VRMWriter);

        BasisNetworkConnection.LocalPlayerPeer.Send(
            VRMWriter,
            BasisNetworkCommons.AudioRecipientsChannel,
            DeliveryMethod.ReliableOrdered
        );

        BasisNetworkProfiler.AddToCounter(BasisNetworkProfilerCounter.AudioRecipients, VRMWriter.Length);
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
        BasisAvatarRecorder.StoreData(
            intervalSeconds,
            anim.bodyRotation,
            anim.bodyPosition,
            BasisNetworkTransmitter.HumanPose.muscles,
            anim.transform.localScale.y
        );
    }
}
