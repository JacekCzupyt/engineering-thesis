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
        public NetworkDictionary<ulong, PlayerState> playerStates = new NetworkDictionary<ulong, PlayerState>();
        public event EventHandler<PlayerManager> SpawnPlayerEvent;
        private PlayerScoreUI playerScoreUI;
        private GameMode gameMode;
        private int teamCount;

        public void Start() {
            //Scoreboard
            playerScoreUI = scoreboardUIObject.GetComponent<PlayerScoreUI>();
            playerScoreUI.gameObject.SetActive(false);

            if(IsClient)
            {
                playerStates.OnDictionaryChanged += HandlePlayerStateChange;
            }
            if(IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
                foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    if(!playerStates.ContainsKey(client.ClientId))
                    {
                        HandleClientConnected(client.ClientId);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            playerStates.OnDictionaryChanged -= HandlePlayerStateChange;

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

            if(!playerManager)
            {
                Debug.Log("No player manager detected for " + clientId);
                //Client connecting after game has started logic
                return;
            }
            SpawnPlayerEvent?.Invoke(this, playerManager);
            Debug.Log("Player with id " + clientId + " has joined the game!");
            playerManager.SetGameManager(this);
            playerStates.Add(clientId, playerManager.ToPlayerState());
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if(playerStates.ContainsKey(clientId))
            {
                Debug.Log("Player with id " + clientId + " has left the game.");  
                playerStates.Remove(clientId);

            }else
            {
                Debug.Log("Player with id " + clientId + ", does not exist.");
            }
        }

        private void HandlePlayerStateChange(NetworkDictionaryEvent<ulong, PlayerState> state)
        {
            ScoreboardUpdate();
        }

        private void ScoreboardUpdate()
        {
            playerScoreUI.DestroyCards();
            int i = 0;
            foreach(var player in playerStates)
            {
                float position = -(30*i + 30*(i+1));
                playerScoreUI.CreateListItem(player.Value, position);
                i++;
            }
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                scoreboardUIObject.SetActive(true);
            }
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                scoreboardUIObject.SetActive(false);
            }
        }
    }
}
