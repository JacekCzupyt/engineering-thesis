using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Profiling;
using UnityEngine;

public class PlayerController : NetworkBehaviour {

    private Rigidbody rb;
    public float snapDistance = 3;
    public float networkPullModifier = 1;
    private int Itemindex;
    private int? tickDelta = null;

    private MovementControls movementControls;

    [SerializeField] Canvas barHealth;

    private void Start() {
        if (!IsLocalPlayer)
        {
            barHealth.enabled = false;
            this.GetComponentInChildren<Camera>().enabled = false;
        }
        // EquipItem(0);

        // em = bulletSystem.emission;
        rb = this.GetComponent<Rigidbody>();
        movementControls = GetComponentInChildren<MovementControls>();
    }

    private void FixedUpdate() {
        if (this.IsOwner)
            movementControls.InputMovement();
    }
}
