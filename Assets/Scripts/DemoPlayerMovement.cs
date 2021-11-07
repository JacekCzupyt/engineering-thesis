using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

public class DemoPlayerMovement : NetworkBehaviour {
    private Rigidbody rb;
    public float acceleration = 2f;
    public float snap = 1;
    public ParticleSystem bulletSystem;
    private ParticleSystem.EmissionModule em;

    [SerializeField] Item[] items;
    private int Itemindex;

    private NetworkVariableVector3 networkVelocity = new NetworkVariableVector3(new NetworkVariableSettings {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.OwnerOnly
    });
    private NetworkVariableVector3 networkAcceleration = new NetworkVariableVector3(new NetworkVariableSettings {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.OwnerOnly
    });
    private NetworkVariableVector3 networkTransform = new NetworkVariableVector3(new NetworkVariableSettings {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.OwnerOnly
    });

    private void Start() {
        if (!IsLocalPlayer)
            this.GetComponentInChildren<Camera>().enabled = false;
        EquipItem(0);

        em = bulletSystem.emission;
        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (!this.IsOwner) {
            if((transform.position - networkTransform.Value).magnitude > snap)
                transform.position = networkTransform.Value;
            rb.velocity = networkVelocity.Value;
            rb.AddForce(networkAcceleration.Value);

            return;
        }

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
        
        if(acc != Vector3.zero)
            acc.Normalize();
        
        rb.AddForce(acc * acceleration);
        networkVelocity.Value = rb.velocity;
        networkAcceleration.Value = acc * acceleration;
        networkTransform.Value = transform.position;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            items[Itemindex].Use();
            em.rateOverTime = 10f;
        }
        else
        {
            em.rateOverTime = 0f;
        }

    }
    private void EquipItem(int index)
    {
        items[index].itemGameObject.SetActive(true);
        Itemindex = index;
    }
}
