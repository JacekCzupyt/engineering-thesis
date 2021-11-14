using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementControls : MonoBehaviour {
    private Rigidbody rb;
    private InputManager input;
    
    [SerializeField] private float driftAcceleration = 5f;

    [SerializeField] private float gripRadius = 1.5f;
    
    
    public enum MovementState {
        Drift,
        Grip
    }

    public MovementState currentState = MovementState.Drift;

    private bool gripActionUsed = false;
    
    public void Start() {
        input = InputManager.Instance;
        rb = GetComponentInParent<Rigidbody>();
    }

    public Vector3 InputMovement() {

        switch (currentState) {
            case MovementState.Drift: {
                Vector3 dir = transform.rotation * input.GetPlayerMovement();
                Vector3 acceleration = dir * driftAcceleration;
                rb.AddForce(acceleration);

                if (input.GetGripAction() == InputActionPhase.Started &&
                    !gripActionUsed &&
                    Physics.OverlapSphere(transform.position, gripRadius, LayerMask.GetMask("Map")).Length > 0) {

                    gripActionUsed = true;
                    currentState = MovementState.Grip;
                }
                
                return acceleration;
            }
            case MovementState.Grip: {
                throw new NotImplementedException();
            }
        }
        throw new ArgumentException("movement state is invalid");
    }
}
