using System;
using System.Diagnostics;
using Game_Systems.Utility;
using Input_Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using Debug = UnityEngine.Debug;

namespace Game_Systems.Equipment {
    public class Grapple : MonoBehaviour {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject anchorPrefab;

        [SerializeField] private float hookVelocity = 30;
        [SerializeField] private float retractionVelocity = 60;
        [SerializeField] private ThrowType throwType = ThrowType.Static;

        private float attachOffset = 0.1f;
        
        private CharacterInputManager input;

        private GameObject anchor;
        private Rigidbody anchorRb;

        public enum GrappleState {
            Ready,
            InFlight,
            Attached,
            Retracting,
            Unavailable
        }

        public GrappleState State { get; set; }

        private void Start() {
            input = CharacterInputManager.Instance;
            input.AbilityAction.started += Fire;
            input.AbilityAction.canceled += Cancel;
        }

        private void FixedUpdate() {
            if (State != GrappleState.Retracting)
                return;
            
            var deltaPos = player.transform.position - anchor.transform.position;
            if (deltaPos.magnitude < retractionVelocity * Time.fixedDeltaTime) {
                RemoveHook();
            }
            else {
                anchorRb.velocity = (player.transform.position - anchor.transform.position).normalized * retractionVelocity;
            }
        }

        private void Update() {
            Debug.Log(State);
            if (State.In(GrappleState.InFlight, GrappleState.Attached)) {
                CheckGrapplePath();
            }
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
                LayerMask.GetMask("Map")
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

        private void Cancel(InputAction.CallbackContext context) {
            if (!State.In(GrappleState.InFlight, GrappleState.Attached, GrappleState.Retracting))
                return;
            State = GrappleState.Retracting;
            anchorRb.isKinematic = false;
            anchorRb.velocity = (player.transform.position - anchor.transform.position).normalized * retractionVelocity;
        }
    }
}
