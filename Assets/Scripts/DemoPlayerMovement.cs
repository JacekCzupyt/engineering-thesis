using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class DemoPlayerMovement : NetworkBehaviour {
    private Rigidbody rb;
    public float acceleration = 2f;

    private void Start() {
        if (!IsLocalPlayer)
            this.GetComponentInChildren<Camera>().enabled = false;
        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (!this.IsOwner)
            return;
        
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(transform.forward * acceleration);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(-transform.forward * acceleration);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(transform.right * acceleration);
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(-transform.right * acceleration);
        if (Input.GetKey(KeyCode.Space))
            rb.AddForce(transform.up * acceleration);
        if (Input.GetKey(KeyCode.LeftShift))
            rb.AddForce(-transform.up * acceleration);
    }
}
