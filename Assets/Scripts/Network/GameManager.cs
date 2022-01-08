using System;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using MLAPI.NetworkVariable;
using UI.Game;
using UnityEngine;

namespace Network
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private GameObject scoreboardUIObject;
        private NetworkDictionary<ulong, PlayerState> playerStates = new NetworkDictionary<ulong, PlayerState>();
        public event EventHandler<PlayerManager> SpawnPlayerEvent;
        public event EventHandler<GameInfo> SendGameInfoEvent;
        private PlayerScoreUI playerScoreUI;
        private NetworkVariable<GameInfo> gameInfo = new NetworkVariable<GameInfo>();
        private int teamCount;

        public void Start() {
            //Scoreboard
            playerScoreUI = scoreboardUIObject.GetComponent<PlayerScoreUI>();
            playerScoreUI.gameObject.SetActive(false);

            if(IsClient)
            {
                playerStates.OnDictionaryChanged += HandlePlayerStateChange;
                gameInfo.OnValueChanged += HandleGameInfoValueChanged;
            }
            if(IsServer)
            {
                //Game Info
                var gameInfoObject = GameObject.FindGameObjectWithTag("GameInfo");
                gameInfo.Value = gameInfoObject.GetComponent<GameInfo>();

                SendGameInfoEvent?.Invoke(this, gameInfo.Value);

                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

                foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                    // if(!playerStates.ContainsKey(client.ClientId))
                    // {
                    // }
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
            playerStates.Add(clientId, playerManager.ToPlayerState());
            playerManager.DestoryPlayerManager();
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

        private void HandleGameInfoValueChanged(GameInfo prevGinfo, GameInfo newGinfo)
        {
            gameInfo.Value = newGinfo;
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayerKillServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
        {
            if(!IsServer)
                throw new InvalidOperationException("This method can only be run on the server");

            if(playerStates.ContainsKey(clientId))
            {
                playerStates[clientId] = new PlayerState(
                    playerStates[clientId].ClientId,
                    playerStates[clientId].PlayerName,
                    playerStates[clientId].TeamId,
                    playerStates[clientId].PlayerKills + 1,
                    playerStates[clientId].PlayerDeaths
                );
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayerDeathServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
        {
            if(playerStates.ContainsKey(clientId))
            {
                playerStates[clientId] = new PlayerState(
                    playerStates[clientId].ClientId,
                    playerStates[clientId].PlayerName,
                    playerStates[clientId].TeamId,
                    playerStates[clientId].PlayerKills,
                    playerStates[clientId].PlayerDeaths + 1
                );
            }
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
                Debug.Log(playerStates.Count);
                Debug.Log(gameInfo.Value.PlayerCount);
            }
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                scoreboardUIObject.SetActive(false);
            }
        }

        public void PlayerKillUpdate(ulong clientId)
        {
            PlayerKillServerRpc(clientId);
        }

        public void PlayerDeathUpdate(ulong clientId)
        {
            PlayerDeathServerRpc(clientId);
        }

        public GameInfo GetGameInfo()
        {
            return gameInfo.Value;
        }
    }
}
