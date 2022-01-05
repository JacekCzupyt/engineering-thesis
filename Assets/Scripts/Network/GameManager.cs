using System;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using UI.Game;
using UnityEngine;

namespace Network
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private GameObject gamestateManagerObject;
        [SerializeField] private GameObject scoreboardUIObject;
        public NetworkList<PlayerState> playersStates = new NetworkList<PlayerState>();
        private PlayerScoreUI playerScoreUI;

        public void Start() {
            playerScoreUI = scoreboardUIObject.GetComponent<PlayerScoreUI>();
            playerScoreUI.gameObject.SetActive(false);
            if(IsClient)
            {
                playersStates.OnListChanged += HandlePlayerStatesChange;
            }
            if(IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
                foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                }
            }
        }

        private void OnDestroy()
        {
            playersStates.OnListChanged -= HandlePlayerStatesChange;

            if(NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            PlayerManager playerManager = null;
            foreach (var manager in GameObject.FindGameObjectsWithTag("PlayerManager")) {
                playerManager = manager.GetComponent<PlayerManager>();
                if(playerManager.GetClientId() == clientId) break;
            }

            if(!playerManager) return;
            Debug.Log(playerManager.name + " has joined the game!");
            playerManager.SetGameManager(this);
            playersStates.Add(playerManager.ToPlayerState());
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            for(int i = 0; i < playersStates.Count; i++)
            {
                if(playersStates[i].ClientId == clientId)
                {
                    Debug.Log(playersStates[i].PlayerName + " has left the game.");
                    playersStates.RemoveAt(i);
                    break;
                }
            }
        }

        private void HandlePlayerStatesChange(NetworkListEvent<PlayerState> states)
        {
            ScoreboardUpdate();
        }

        private void ScoreboardUpdate()
        {
            playerScoreUI.DestroyCards();
            for(int i = 0; i < playersStates.Count; i++)
            {
                float position = -(30*i + 30*(i+1));
                playerScoreUI.CreateListItem(playersStates[i], position);
            }
        }
    }
}
