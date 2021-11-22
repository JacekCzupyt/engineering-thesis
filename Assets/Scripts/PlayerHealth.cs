using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] NetworkVariableInt health = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, 100);
    PlayerRespawn respawnPlayer;
    [SerializeField] HealthBar bar;
    // Start is called before the first frame update

    // Update is called once per frame

    private void Start()
    {
        respawnPlayer = GetComponent<PlayerRespawn>();
        bar.SetInitialHealth();
    }
    void Update()
    {
        if(IsOwner && health.Value<=0)
        {
            health.Value = 100;
            //bar.SetInitialHealth(100);
            respawnPlayer.Respawn();
        }

    }
    public void takeDemage(int damage)
    {
        Debug.Log("applyDemage");
        health.Value = health.Value-damage;
        bar.SetHealth(health.Value);

    }
}
