using System;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Utility;

namespace Network {
    public class NetworkRotation : NetworkBehaviour {
        [SerializeField] private float tickRate = 20;
        [SerializeField] private bool interpolate = true;
        [SerializeField] private float minAngle = 1f;

        private float lastReceiveTime;
        private Quaternion lerpStartRot;
        private Quaternion lerpEndRot;
        private float lerpTime;
        private Transform playerTransform;

        private float lastSendTime;
        private Quaternion lastSentRot;
        
        private void Start() {
            playerTransform = transform.parent;
        }

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
                !(Quaternion.Angle(playerTransform.rotation, lastSentRot) > minAngle))
                return;
        
            lastSendTime = NetworkManager.Singleton.NetworkTime;
            lastSentRot = playerTransform.rotation;

            if (IsServer) {
                SubmitRotationClientRpc(
                    playerTransform.rotation,
                    this.NonOwnerClientParams()
                );
            }
            else {
                SubmitRotationServerRpc(playerTransform.rotation);
            }
        }

        /// <summary>
        /// Interpolate the rotation from received data
        /// </summary>
        private void InterpolateRotation() {
            float sendDelay = 1f / tickRate;
            lerpTime += Time.unscaledDeltaTime / sendDelay;

            playerTransform.rotation = Quaternion.Slerp(lerpStartRot, lerpEndRot, lerpTime);
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
                lerpStartRot = playerTransform.rotation;
                lerpEndRot = rotation;
                lerpTime = 0;
            }
            else {
                playerTransform.rotation = rotation;
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
                this.NonOwnerClientParams()
            );
        }

        [ClientRpc]
        private void SubmitRotationClientRpc(Quaternion rotation, ClientRpcParams rpcParams = default) {
            if (enabled) {
                ApplyRotationInternal(rotation);
            }
        }
    }
}
