using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable.Collections;
using MLAPI.Connection;
using MLAPI.Messaging;

public class PlayerScoreManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerScorePanel;

    private NetworkList<string> playersNames = new NetworkList<string>();


    public override void NetworkStart()
    {
        foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Debug.Log(client.ClientId);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            playerScorePanel.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            playerScorePanel.SetActive(false);
        }
    }


}
