using Basis.Network.Core;
using Basis.Scripts.BasisSdk;
using Basis.Scripts.BasisSdk.Players;
using Basis.Scripts.Networking;
using Basis.Scripts.Networking.NetworkedAvatar;
using Basis.Scripts.Networking.Receivers;
using Basis.Scripts.Networking.Transmitters;
using Basis.Scripts.Profiler;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using static SerializableBasis;
[System.Serializable]
public partial class BasisTransmissionResults
{

    public int LengthOfArrays = -1;
    public List<ushort> TalkingPoints = new List<ushort>(128);
    public float intervalSeconds = 0.05f;
    public float timer = 0f;
    public float SquaredSmallestDistance;
    public float UnClampedInterval;
    public float DefaultInterval;

    public bool AnyMicrophoneRangeChanged;
    public bool AnyHearingRangeChanged;
    public bool AnyAvatarRangeChanged;

    [SerializeReference]
    public BasisNetworkTransmitter BasisNetworkTransmitter;
    public NetDataWriter VRMWriter = new NetDataWriter(true, 0);
    public VoiceReceiversMessage VRM = new VoiceReceiversMessage();
    /// <summary>
    /// Called each frame; drives scheduling of distance job and network sync.
    /// </summary>
    public void Simulate()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;

        if (timer <= intervalSeconds)
        {
            return;
        }

        // Use the actual accumulated interval (handles overshoot)
        float previousInterval = intervalSeconds;
        if (CanDoSimulate(previousInterval, out BasisAvatar avatar) == false)
        {
            return;
        }

        int receiverCount = BasisNetworkPlayers.ReceiverCount;
        var snapshot = BasisNetworkPlayers.ReceiversSnapshot;
        if (LengthOfArrays != receiverCount)
        {
            ResizeOrCreateArrayData(receiverCount);//resets arrays and resizes
        }
        // Compress avatar state (doesn't touch mouth bone used as input)
        BasisNetworkAvatarCompressor.Compress(BasisNetworkTransmitter, avatar.Animator);


        /// AnyMicrophoneRangeChanged AnyHearingRangeChanged AnyAvatarRangeChanged AnyIdOrderOrLengthChanged;
        bool worked = RemoteBoneJobSystem.TryGetAnyChanged(out AnyMicrophoneRangeChanged, out AnyHearingRangeChanged, out AnyAvatarRangeChanged);
        if (worked == false)
        {
            BasisDebug.LogError("unable to get any changed data!");
            return;
        }
        worked = RemoteBoneJobSystem.TryGetSmallestDistanceSq(out SquaredSmallestDistance);
        if (worked == false)
        {
            BasisDebug.LogError("unable to Squared Smallest Distance!");
            return;
        }

        bool MicrophoneChange = IndexChanged || AnyMicrophoneRangeChanged;
        bool HearingChange = IndexChanged || AnyHearingRangeChanged;
        bool AvatarChange = IndexChanged || AnyAvatarRangeChanged;

        if (HearingChange)
        {
            for (int index = 0; index < receiverCount; index++)
            {
                var receiver = snapshot[index];
                if (RemoteBoneJobSystem.TryGetHearingRange(receiver.playerId, out bool canHear))
                {
                    if (receiver.AudioReceiverModule.HasAudioSource != canHear)
                    {
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
                else
                {
                    BasisDebug.LogError("Cant Get hearing Range!");
                }
            }
        }
        if (AvatarChange)
        {
            for (int index = 0; index < receiverCount; index++)
            {
                var receiver = snapshot[index];
                var remote = receiver.RemotePlayer;
                if (remote.IsLoadingAnAvatar == false)
                {
                    if (RemoteBoneJobSystem.TryGetAvatarRange(receiver.playerId, out bool HasAvatar))
                    {
                        if (remote.InAvatarRange != HasAvatar)
                        {
                            remote.InAvatarRange = HasAvatar;
                            remote.ReloadAvatar();
                        }
                    }
                    else
                    {
                        BasisDebug.LogError("Cant Get Avatar Range!");
                    }
                }
            }
        }
        float MeshLodMulitplier = SMModuleDistanceBasedReductions.MeshLod;
        for (int index = 0; index < receiverCount; index++)
        {
            var receiver = snapshot[index];
            var remote = receiver.RemotePlayer;
            if (RemoteBoneJobSystem.TryGetDistanceSq(receiver.playerId, out float Distance))
            {
                // Distance-based mesh LOD
                remote.ChangeMeshLOD(Distance, MeshLodMulitplier);
            }
            else
            {
                BasisDebug.LogError("Cant Get Avatar Range!");
            }
        }

        //update the server with who we are talking to
        if (MicrophoneChange)
        {
            if (TalkingPoints.Capacity < receiverCount)
            {
                TalkingPoints.Capacity = receiverCount;
            }
            TalkingPoints.Clear();
            for (int index = 0; index < receiverCount; index++)
            {
                BasisNetworkReceiver remote = snapshot[index];
                if (RemoteBoneJobSystem.TryGetMicrophoneRange(remote.playerId, out bool HasMicrophoneRange))
                {
                    if (HasMicrophoneRange)
                    {
                        ushort ID = remote.playerId;
                        TalkingPoints.Add(ID);
                    }
                    else
                    {
                        BasisDebug.LogError("Cant Get Microphone Range!");
                    }
                }
            }
            BasisNetworkTransmitter.HasReasonToSendAudio = TalkingPoints.Count != 0;
            VRM.Users = TalkingPoints.ToArray();
            VRMWriter.Reset();
            VRM.Serialize(VRMWriter);
            //BasisDebug.Log($"Sending Microphone Check Data ({AudioRecipientswriter.Length})", BasisDebug.LogTag.Voice);
            BasisNetworkConnection.LocalPlayerPeer.Send(VRMWriter, BasisNetworkCommons.AudioRecipientsChannel, DeliveryMethod.ReliableOrdered);

            BasisNetworkProfiler.AddToCounter(BasisNetworkProfilerCounter.AudioRecipients, VRMWriter.Length);
        }
        if (!float.IsFinite(SquaredSmallestDistance))
        {
            // No receivers or all failed positions => treat as zero for rate calc
            SquaredSmallestDistance = 0f;
        }

        ServerMetaDataMessage ServerMetaDataMessage = BasisNetworkManagement.ServerMetaDataMessage;
        DefaultInterval = ServerMetaDataMessage.SyncInterval / 1000f;


        float calculatedIntervalBase = ServerMetaDataMessage.BaseMultiplier + (SquaredSmallestDistance * ServerMetaDataMessage.IncreaseRate);
        UnClampedInterval = DefaultInterval * calculatedIntervalBase;

        intervalSeconds = Mathf.Clamp(UnClampedInterval, DefaultInterval, ServerMetaDataMessage.SlowestSendRate);

        if (BasisAvatarRecorder.IsRecording)
        {
            var Anim = avatar.Animator;
            BasisAvatarRecorder.StoreData(intervalSeconds, Anim.bodyRotation, Anim.bodyPosition, BasisNetworkTransmitter.HumanPose.muscles, Anim.transform.localScale.y);
        }
        IndexChanged = false;
        // account for overshoot using the interval that actually accumulated
        timer -= previousInterval;
    }
    /// <summary>
    /// Allocate / reallocate all NativeArrays.
    /// </summary>
    public void ResizeOrCreateArrayData(int receiverCount)
    {
        LengthOfArrays = receiverCount;
    }
    public bool CanDoSimulate(float previousInterval, out BasisAvatar BasisAvatar)
    {
        var player = BasisNetworkTransmitter.Player;
        if (player != null)
        {
            BasisAvatar = player.BasisAvatar;
        }
        else
        {
            BasisAvatar = null;
        }
        if (BasisAvatar == null)
        {
            BasisDebug.LogError("Missing Basis Avatar. Cannot send network update.", BasisDebug.LogTag.System);
            timer -= previousInterval;
            return false;
        }
        return true;
    }
    public void Initalize()
    {
        BasisNetworkPlayer.OnRemotePlayerJoined += OnPlayerIndexChanged;
    }
    public void DeInitalize()
    {
        BasisNetworkPlayer.OnRemotePlayerLeft -= OnPlayerIndexChanged;
    }
    public bool IndexChanged;
    public void OnPlayerIndexChanged(BasisNetworkPlayer BNP, BasisRemotePlayer BRP)
    {
        IndexChanged = true;
    }
}
