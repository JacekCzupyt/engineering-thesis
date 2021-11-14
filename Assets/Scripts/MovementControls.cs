using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour {
    private Rigidbody rb;
    private InputManager input;
    
    [SerializeField] private float driftAcceleration = 5f;
    
    
    public enum MovementState {
        Drift,
        Grip
    }

    public MovementState currentState = MovementState.Drift;

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
                return acceleration;
            }
            case MovementState.Grip: {
                throw new NotImplementedException();
            }
        }
        throw new ArgumentException("movement state is invalid");
    }
}
