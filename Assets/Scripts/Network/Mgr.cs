using System;
using System.Linq;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using MLAPI.NetworkVariable;
using NetPortals;
using UI.Lobby;
using UnityEngine;

public class Mgr : NetworkBehaviour
{
    public static Mgr Instance => Instance;
    private static Mgr instance;

    private void Awake() {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void NetworkStart()
    {
        if(IsClient)
        {
            Debug.Log("Client");
        }

        if(IsServer)
        {
            Debug.Log("Server");
        }
    }


}
