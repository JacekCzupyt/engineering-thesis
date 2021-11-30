using System;
using System.Diagnostics;
using Game_Systems.Utility;
using Input_Systems;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using Debug = UnityEngine.Debug;

namespace Game_Systems.Equipment {
    public class Grapple : NetworkBehaviour {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject anchorPrefab;

        [SerializeField] private ThrowType throwType = ThrowType.Static;
        [SerializeField] private float hookVelocity = 30;
        [SerializeField] private float retractionVelocity = 60;
        [SerializeField] private float maxHookDistance = 60;

        [Tooltip("Force that will be applied when player is moving towards the grapple point.")] [SerializeField]
        private float maxReelForce = 10f;

        [Tooltip("Force that will be applied when player is moving away from the grapple point.")] [SerializeField]
        private float maxBreakForce = 20f;

        private float attachOffset = 0.001f;

        private CharacterInputManager input;

        private GameObject anchor;
        private Rigidbody anchorRb;

        private Rigidbody rb;

        public enum GrappleState {
            Ready,
            InFlight,
            Attached,
            Retracting,
            Unavailable
        }

        public GrappleState State { get; set; }

        private void Start() {
            if (!IsOwner)
                return;

            input = CharacterInputManager.Instance;
            input.AbilityAction.started += Fire;
            input.AbilityAction.canceled += CancelGrapple;
            rb = player.GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            if (!IsOwner)
                return;
            
            if (!State.In(GrappleState.Attached, GrappleState.Retracting, GrappleState.InFlight))
                return;

            if (State.In(GrappleState.InFlight, GrappleState.Attached)) {
                CheckGrapplePath();
            }

            if (State == GrappleState.Attached)
                ReelIn();

            var deltaPos = player.transform.position - anchor.transform.position;

            if (State == GrappleState.InFlight && deltaPos.magnitude > maxHookDistance) {
                Retract();
            }

            if (State == GrappleState.Retracting) {
                if (deltaPos.magnitude < retractionVelocity * Time.fixedDeltaTime) {
                    RemoveHook();
                }
                else {
                    anchorRb.velocity = (player.transform.position - anchor.transform.position).normalized * retractionVelocity;
                }
            }
        }

        private void ReelIn() {
            var dir = (anchor.transform.position - player.transform.position).normalized;
            var force = dir *
                (Vector3.Dot(dir, rb.velocity) > 0 ?
                    maxReelForce :
                    maxBreakForce);
            rb.AddForce(force, ForceMode.Force);
        }

        private void CheckGrapplePath() {
            var pos = player.transform.position;
            var dir = anchor.transform.position - pos;
            //TODO: currently only connects to terrain, not other players
            if (!Physics.Raycast(
                pos,
                dir,
                out var hit,
                dir.magnitude - attachOffset,
                LayerMask.GetMask("Terrain")
            ))
                return;

            anchorRb.isKinematic = true;
            anchor.transform.position = hit.point;
            State = GrappleState.Attached;
        }

        private void RemoveHook() {
            GameObject.Destroy(anchor);
            State = GrappleState.Ready;
        }

        private void Fire(InputAction.CallbackContext context) {
            if (State != GrappleState.Ready)
                return;
            State = GrappleState.InFlight;
            anchor = ObjectThrower.Throw(throwType)(player.transform, player.transform.forward, anchorPrefab, hookVelocity);
            anchorRb = anchor.GetComponent<Rigidbody>();
        }

        private void CancelGrapple(InputAction.CallbackContext context) {
            Retract();
        }

        private void Retract() {
            if (!State.In(GrappleState.InFlight, GrappleState.Attached))
                return;
            State = GrappleState.Retracting;
            anchorRb.isKinematic = false;
        }
    }
}
