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
    [SerializeField] private float gripAcceleration = 20f;
    [SerializeField] private float gripDrag = 4f;
    [SerializeField] private float gripDragExponent = 1.4f;
    
    
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
        Debug.Log(currentState);

        if (input.GetGripAction() != InputActionPhase.Started)
            gripActionUsed = false;

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
                Vector3 dir = transform.rotation * input.GetPlayerMovement();
                //TODO: change to velocity relative to gripped object if movable maps are implemented
                //TODO: cap drag deceleration
                Vector3 acceleration = dir * gripAcceleration - rb.velocity.normalized * (Mathf.Pow(rb.velocity.magnitude, gripDragExponent) * gripDrag);
                rb.AddForce(acceleration);
                
                if ((input.GetGripAction() == InputActionPhase.Started &&
                    !gripActionUsed) ||
                    Physics.OverlapSphere(transform.position, gripRadius, LayerMask.GetMask("Map")).Length == 0) {

                    gripActionUsed = true;
                    currentState = MovementState.Drift;
                }
                return acceleration;
            }
        }
        throw new ArgumentException("movement state is invalid");
    }
}
