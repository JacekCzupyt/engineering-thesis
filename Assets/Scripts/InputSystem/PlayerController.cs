using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Profiling;
using UnityEngine;

public class PlayerController : NetworkBehaviour {

    private Rigidbody rb;
    public float acceleration = 2f;
    public float snapDistance = 3;
    private InputManager inputManager;
    public float networkPullModifier = 1;
    public ParticleSystem bulletSystem;
    private ParticleSystem.EmissionModule em;
    [SerializeField] Item[] items;
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
        // EquipItem(0);

        // em = bulletSystem.emission;
        rb = this.GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
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

    private void InputMovement(){
        Vector3 acc = inputManager.GetPlayerMovement();
        
        acc = transform.rotation * acc;

        rb.AddForce(acc * acceleration);
        if (!Input.GetKey(KeyCode.Mouse1)) {
            networkPosition.Value = transform.position;
            networkVelocity.Value = rb.velocity;
            networkAcceleration.Value = acc * acceleration;
        }
    }

}