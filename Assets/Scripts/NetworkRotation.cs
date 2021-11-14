using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public class NetworkRotation : NetworkBehaviour {
    [SerializeField] private float tickRate = 20;
    [SerializeField] private bool interpolate = true;
    [SerializeField] private float minAngle = 1f;

    private float lastReceiveTime;
    private Quaternion lerpStartRot;
    private Quaternion lerpEndRot;
    private float lerpTime;

    private float lastSendTime;
    private Quaternion lastSentRot;

    /// <summary>
    /// Rpc params that send data to all clients, except the owner of the object
    /// </summary>
    private ClientRpcParams NonOwnerClientParams =>
        new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = NetworkManager.Singleton.ConnectedClientsList.Where(c => c.ClientId != OwnerClientId)
                    .Select(c => c.ClientId).ToArray()
            }
        };

    private void Update() {
        //TODO: merge with mouse look script?
        if (IsOwner) {
            SendData();
        }
        else if (IsClient && interpolate) {
            InterpolateRotation();
        }
    }

    /// <summary>
    /// Sends data to the server, or to all clients if this is the server
    /// </summary>
    private void SendData() {
        //Only send if min time has passed and min angle change has been reached
        if (!(NetworkManager.Singleton.NetworkTime - lastSendTime >= (1f / tickRate)) ||
            !(Quaternion.Angle(transform.rotation, lastSentRot) > minAngle))
            return;
        
        lastSendTime = NetworkManager.Singleton.NetworkTime;
        lastSentRot = transform.rotation;

        if (IsServer) {
            SubmitRotationClientRpc(
                transform.rotation,
                NonOwnerClientParams
            );
        }
        else {
            SubmitRotationServerRpc(transform.rotation);
        }
    }

    /// <summary>
    /// Interpolate the rotation from received data
    /// </summary>
    private void InterpolateRotation() {
        float sendDelay = 1f / tickRate;
        lerpTime += Time.unscaledDeltaTime / sendDelay;

        transform.rotation = Quaternion.Slerp(lerpStartRot, lerpEndRot, lerpTime);
    }

    /// <summary>
    /// Save new target rotation to interpolate to
    /// </summary>
    /// <param name="rotation"></param>
    private void ApplyRotationInternal(Quaternion rotation) {
        if (!enabled)
            return;

        if (interpolate && IsClient) {
            lastReceiveTime = Time.unscaledTime;
            lerpStartRot = transform.rotation;
            lerpEndRot = rotation;
            lerpTime = 0;
        }
        else {
            transform.rotation = rotation;
        }
    }
    
    [ServerRpc]
    private void SubmitRotationServerRpc(Quaternion rotation, ServerRpcParams rpcParams = default) {
        if (!enabled)
            return;

        if (!IsClient) {
            // Dedicated server
            ApplyRotationInternal(rotation);
        }

        SubmitRotationClientRpc(
            rotation,
            NonOwnerClientParams
        );
    }

    [ClientRpc]
    private void SubmitRotationClientRpc(Quaternion rotation, ClientRpcParams rpcParams = default) {
        if (enabled) {
            ApplyRotationInternal(rotation);
        }
    }
}