using System;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using MLAPI.NetworkVariable;
using UI.Game;
using UnityEngine;
using Game_Systems;

namespace Network
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private GameObject scoreboardUIObject;
        [SerializeField] private GameObject playerSpawnerObject;
        private NetworkList<PlayerState> playerStates = new NetworkList<PlayerState>();
        private NetworkVariable<GameInfo> gameInfo = new NetworkVariable<GameInfo>();
        private PlayerScoreUI playerScoreUI;
        private GameObject gameInfoObject;

        public void Start() {
            //Scoreboard
            playerScoreUI = scoreboardUIObject.GetComponent<PlayerScoreUI>();
            playerScoreUI.gameObject.SetActive(false);

            if(IsClient)
            {
                playerStates.OnListChanged += HandlePlayerStateChange;
                gameInfo.OnValueChanged += HandleGameInfoChange;
                UpdateGameMode();
                ScoreboardUpdate();
            }
            if(IsServer)
            {
                //Game Info
                gameInfoObject = GameObject.FindGameObjectWithTag("GameInfoManager");
                gameInfo.Value = gameInfoObject.GetComponent<GameInfoManager>().GetGameInfo();

                playerSpawnerObject.GetComponent<PlayerSpawner>()
                .ReceiveGameInfo(gameInfo.Value);

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
            playerStates.OnListChanged -= HandlePlayerStateChange;
            gameInfo.OnValueChanged -= HandleGameInfoChange;

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
            playerSpawnerObject.GetComponent<PlayerSpawner>().SpawnPlayer(playerManager);
            Debug.Log("Player with id " + clientId + " has joined the game!");
            playerStates.Add(playerManager.ToPlayerState());
            //playerManager.DestoryPlayerManager();
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            for(int i = 0; i < playerStates.Count; i++)
            {
                if(playerStates[i].ClientId == clientId)
                {
                    Debug.Log("Player with id " + clientId + " has left the game.");  
                    playerStates.RemoveAt(i);
                }
            }
        }

        private void HandlePlayerStateChange(NetworkListEvent<PlayerState> state)
        {
            ScoreboardUpdate();
        }

        private void HandleGameInfoChange(GameInfo prevInfo, GameInfo newInfo)
        {
            UpdateGameMode();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayerKillServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
        {
            if(!IsServer)
                throw new InvalidOperationException("This method can only be run on the server");

            for(int i = 0; i < playerStates.Count; i++)
            {
                if(playerStates[i].ClientId == clientId)
                {
                    playerStates[i] = new PlayerState(
                        playerStates[i].ClientId,
                        playerStates[i].PlayerName,
                        playerStates[i].TeamId,
                        playerStates[i].PlayerKills + 1,
                        playerStates[i].PlayerDeaths
                    );
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayerDeathServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
        {
            for(int i = 0; i < playerStates.Count; i++)
            {
                if(playerStates[i].ClientId == clientId)
                {
                    playerStates[i] = new PlayerState(
                        playerStates[i].ClientId,
                        playerStates[i].PlayerName,
                        playerStates[i].TeamId,
                        playerStates[i].PlayerKills,
                        playerStates[i].PlayerDeaths + 1
                    );
                }
            }
        }

        private void ScoreboardUpdate()
        {
            playerScoreUI.DestroyCards();
            playerScoreUI.DeleteScores();
            
            if(gameInfo.Value.gameMode == GameMode.FreeForAll)
            {
                int i = 0;
                foreach(var player in playerStates)
                {
                    float position = -(30*i + 30*(i+1));
                    playerScoreUI.CreateListItem(player, position,
                    NetworkManager.Singleton.LocalClientId == player.ClientId);
                    i++;
                }
            }else if(gameInfo.Value.gameMode == GameMode.TeamDeathmatch)
            {
                for(int i = 1; i < gameInfo.Value.teamCount + 1; i++)
                {
                    int k = 0;
                    int teamSeparator = 600/gameInfo.Value.teamCount;
                    foreach(var player in playerStates)
                    {
                        if(player.TeamId == i)
                        {
                            float position = -(30*k + 20*(k+1) + (i-1) * teamSeparator);
                            playerScoreUI.CreateListItem(player, position,
                            NetworkManager.Singleton.LocalClientId == player.ClientId);
                            
                            k++;
                        }
                    }
                    playerScoreUI.AddTeamScores(i, GetTeamScore(i));
                }
            }
        }

        private void UpdateGameMode()
        {
            playerScoreUI.UpdateGameMode(gameInfo.Value.gameMode);
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
            return gameInfoObject.GetComponent<GameInfoManager>().GetGameInfo();
        }

        public int GetTeamScore(int teamId)
        {
            int score = 0;
            foreach(var player in playerStates)
            {
                if(player.TeamId == teamId) score += player.PlayerKills;
            }
            return score;
        }

        public PlayerState GetTopPlayerScore()
        {
            PlayerState topPlayer = playerStates[0];
            foreach(var player in playerStates)
            {
                if(player.PlayerKills >= topPlayer.PlayerKills)
                {
                    topPlayer = player;
                }
            }
            return topPlayer;
        }
    }
}
