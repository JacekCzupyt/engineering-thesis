using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
public class PlayerHealth : NetworkBehaviour
{
    NetworkVariableInt health = new NetworkVariableInt(100);
    public int userHealth=100;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        userHealth = health.Value;
    }
    public void takeDemage(int damage)
    {
        health.Value = health.Value -damage;
    }
}
