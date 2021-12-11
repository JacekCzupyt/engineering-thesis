using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using Network;
using Input_Systems;
using UnityEngine.UI;

public class CheckGameState : NetworkBehaviour
{

    private PlayerController cc;
    private Renderer[] renderers;
    [SerializeField] Behaviour[] scripts;
    [SerializeField] private GameObject canvas;
    CapsuleCollider playerCollider;

    [SerializeField] ScoreSystem score;
    [SerializeField] Text can;
    // Start is called before the first frame update


    // Update is called once per frame
    
    void Update()
    {
        checkUserScoreServerRPC();
    }
    [ServerRpc]
    private void checkUserScoreServerRPC()
    {
        if (score.userScore.Value >= 5)
        {
            
           var d = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject
                    .GetComponent<PlayerManager>().playerCharacter.Value.GetComponentInChildren<ScoreSystem>().userScore.Value;
            endGameClientRPC(d);
        }
            
       
    }
    [ClientRpc]
    private void endGameClientRPC(int clientId)
    {
        //string win = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent <>
        StartCoroutine(WaitForGameEnd("dupa",clientId));
    }
    IEnumerator WaitForGameEnd(string winner,int score)
    {
        can.text = "Player " + winner + " win a game "+score;
        cc.enabled = false;
        playerCollider.enabled = false;
        if (IsOwner)
        {
            canvas.SetActive(false);
        }
        PlayerState(false);
        can.enabled = true;
        yield return new WaitForSeconds(5);
        can.enabled = false;
        GameNetPortal.Instance.RequestDisconnect();
    }
    private void PlayerState(bool state)
    {
        foreach (var script in scripts)
        {
            script.enabled = state;
        }
        foreach (var render in renderers)
        {
            render.enabled = state;
        }
    }
}
