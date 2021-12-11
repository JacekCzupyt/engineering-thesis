using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable.Collections;
using MLAPI.Connection;
using MLAPI.Messaging;
using UnityEngine.UI;

public class PlayerScoreManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerScorePanel;
    [SerializeField] private GameObject playerScoreCard;
    private NetworkList<PlayerScore> playerScores = new NetworkList<PlayerScore>();
    private List<GameObject> playerCards = new List<GameObject>();

    public override void NetworkStart()
    {   if(IsClient)
        {
            playerScores.OnListChanged += HandleGamePlayersScoreChanged;
        }

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
            
            for(int i = 0; i < playerScores.Count; i++)
            {
                float position = -(30*i + 20*(i+1));
                InitializePlayerCard(playerScores[i], position);
            }
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

        if(!playerData.HasValue) return;

        playerScores.Add(new PlayerScore(
            clientId, 
            playerData.Value.PlayerName,
            0,
            0
        ));
    }

    //Change for using new input system
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

    private void HandleGamePlayersScoreChanged(NetworkListEvent<PlayerScore> scoreState)
    {
        for(int i = 0; i < playerScores.Count; i++)
        {
            float position = -(30*i + 20*(i+1));
            InitializePlayerCard(playerScores[i], position);
        }
        //Update Scores
    }

    private void InitializePlayerCard(PlayerScore score, float position)
    {
        GameObject obj = Instantiate(playerScoreCard, new Vector3(0, position, 0), Quaternion.identity) as GameObject;
        obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = score.PlayerName;
        obj.transform.GetChild(1).gameObject.GetComponent<Text>().text = score.KillScore.ToString();
        obj.transform.GetChild(2).gameObject.GetComponent<Text>().text = score.DeathScore.ToString();
        obj.transform.SetParent(playerScorePanel.transform);
        obj.SetActive(true);
        playerCards.Add(obj);
    }




}
