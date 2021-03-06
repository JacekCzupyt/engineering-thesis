using System;
using Game_Systems.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_Systems {
    public class MovementControls : MonoBehaviour {
        private Rigidbody rb;
        private CharacterInputManager input;
        
        [Header("Grip Movement")]
        [SerializeField] private float gripRadius = 1.5f;
        [SerializeField] private float gripHoldRadius = 2.5f;
        [SerializeField] private float gripAcceleration = 20f;
        [SerializeField] private float gripDrag = 4f;
        [SerializeField] private float gripDragExponent = 1.4f;
        [SerializeField] private Light gripLight;

        private float jumpCharge = 0;
        [Header("Jump Movement")]
        [SerializeField] private float jumpChargeTime = 1;
        [SerializeField] private float minJumpSpeed = 1f;
        [SerializeField] private float maxJumpSpeed = 10f;

        private Jetpack jetpack;


        public enum MovementState {
            Drift,
            Grip,
            Jump
        }

        [Header("Active Values")]
        public MovementState currentState = MovementState.Drift;

        private bool gripActionUsed = false;
    
        public void Start() {
            input = CharacterInputManager.Instance;
            rb = GetComponentInParent<Rigidbody>();
            jetpack = GetComponent<Jetpack>();
            gripLight.color = Color.cyan;
        }

        private void FixedUpdate() {
            InputMovement();
        }

        public Vector3 InputMovement() {
            // Debug.Log(currentState);

            if (input.GetGripAction() != InputActionPhase.Started)
                gripActionUsed = false;

            switch (currentState) {
                case MovementState.Drift: {
                    var force = jetpack.UseJetpack(input.GetPlayerMovement());

                    if (input.GetGripAction() == InputActionPhase.Started &&
                        !gripActionUsed &&
                        Physics.OverlapSphere(transform.position, gripRadius, LayerMask.GetMask("Terrain")).Length > 0) {

                        gripActionUsed = true;
                        currentState = MovementState.Grip;
                        gripLight.color = Color.green;
                    }

                    return force;
                }
                case MovementState.Grip: {
                    Vector3 dir = transform.rotation * input.GetPlayerMovement();
                    Vector3 acceleration = dir * gripAcceleration - rb.velocity.normalized * (Mathf.Pow(rb.velocity.magnitude, gripDragExponent) * gripDrag);
                    rb.AddForce(acceleration);
                
                    if ((input.GetGripAction() == InputActionPhase.Started &&
                            !gripActionUsed) ||
                        Physics.OverlapSphere(transform.position, gripHoldRadius, LayerMask.GetMask("Terrain")).Length == 0) {

                        gripActionUsed = true;
                        currentState = MovementState.Drift;
                        gripLight.color = Color.cyan;
                    }

                    if (input.GetJumpAction() == InputActionPhase.Started) {
                        currentState = MovementState.Jump;
                    }
                
                    return acceleration;
                }
                case MovementState.Jump: {
                    Vector3 acceleration = - rb.velocity.normalized * (Mathf.Pow(rb.velocity.magnitude, gripDragExponent) * gripDrag);
                    rb.AddForce(acceleration);
                
                    if (input.GetJumpAction() == InputActionPhase.Started) {
                        jumpCharge += Time.fixedDeltaTime / jumpChargeTime;
                        jumpCharge = Mathf.Min(jumpCharge, 1f);
                        gripLight.color = Color.Lerp(Color.green, Color.yellow, jumpCharge);
                    }
                    else {
                        var dir = input.GetPlayerMovement();
                        if (dir == Vector3.zero)
                            dir = Vector3.forward;
                        rb.AddForce(transform.rotation * dir * Mathf.Lerp(minJumpSpeed, maxJumpSpeed, jumpCharge), ForceMode.VelocityChange);
                        currentState = MovementState.Drift;
                        gripLight.color = Color.cyan;
                        jumpCharge = 0;
                        return acceleration;
                    }

                    if (input.GetGripAction() == InputActionPhase.Started && !gripActionUsed) {
                        gripActionUsed = true;
                        currentState = MovementState.Grip;
                        gripLight.color = Color.green;
                        jumpCharge = 0;
                    }
                
                    return acceleration;
                }
            }
            throw new ArgumentException("movement state is invalid");
        }
    }
}
