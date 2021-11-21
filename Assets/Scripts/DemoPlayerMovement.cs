using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Profiling;
using UnityEngine;

public class DemoPlayerMovement : NetworkBehaviour {
    private Rigidbody rb;
    public float acceleration = 2f;
    public float snapDistance = 3;
    public float networkPullModifier = 1;

   // [SerializeField] Item[] items;
    private int Itemindex;

    private int? tickDelta = null;

    private NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(
        new NetworkVariableSettings {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.OwnerOnly
        }
    );

    private NetworkVariable<Vector3> networkVelocity = new NetworkVariable<Vector3>(
        new NetworkVariableSettings {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.OwnerOnly
        }
    );
    
    private NetworkVariable<Vector3> networkAcceleration = new NetworkVariable<Vector3>(
        new NetworkVariableSettings {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.OwnerOnly
        }
    );

    private void Start() {
        if (!IsLocalPlayer)
            this.GetComponentInChildren<Camera>().enabled = false;
        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (this.IsOwner)
            InputMovement();
        else
            NetworkMovement();
    }

    private void NetworkMovement() {
        if (!tickDelta.HasValue && networkPosition.LocalTick != NetworkTickSystem.NoTick)
            tickDelta = (CurrentTick - networkPosition.LocalTick) % NetworkTickSystem.TickPeriod;
        
        float timeDelta;
        if (networkPosition.LocalTick == NetworkTickSystem.NoTick) {
            timeDelta = 0;
        }
        else {
            var currentTickDelta = (CurrentTick - networkPosition.LocalTick) % NetworkTickSystem.TickPeriod;
            tickDelta = Math.Min(tickDelta.Value, currentTickDelta);
            timeDelta = (currentTickDelta - tickDelta.Value) * NetworkManager.NetworkConfig.NetworkTickIntervalSec;
        }

        var predictedPosition = 
            networkPosition.Value + 
            networkVelocity.Value * timeDelta +
            networkAcceleration.Value * (timeDelta * timeDelta) / 2;

        if ((predictedPosition - transform.position).magnitude > snapDistance)
            transform.position = predictedPosition;

        var posDiff = (predictedPosition - transform.position);

        rb.velocity = 
            networkVelocity.Value + 
            networkAcceleration.Value * timeDelta +
            posDiff.normalized * (float) (Math.Pow(posDiff.magnitude, 2) * networkPullModifier);
        
        rb.AddForce(networkAcceleration.Value);

        //todo: decrease desired accuracy with relative speed? 
    }

    private void InputMovement() {
        Vector3 acc = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            acc += transform.forward;
        if (Input.GetKey(KeyCode.S))
            acc -= transform.forward;
        if (Input.GetKey(KeyCode.D))
            acc += transform.right;
        if (Input.GetKey(KeyCode.A))
            acc -= transform.right;
        if (Input.GetKey(KeyCode.Space))
            acc += transform.up;
        if (Input.GetKey(KeyCode.LeftShift))
            acc -= transform.up;

        if (acc != Vector3.zero)
            acc.Normalize();

        rb.AddForce(acc * acceleration);
        if (!Input.GetKey(KeyCode.Mouse1)) {
            networkPosition.Value = transform.position;
            networkVelocity.Value = rb.velocity;
            networkAcceleration.Value = acc * acceleration;
        }
    }
}
