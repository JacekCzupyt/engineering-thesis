using System;
using System.Linq;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using MLAPI.NetworkVariable;
using UI.Game;
using UI.Hud;
using UnityEngine;
using Game_Systems;
using System.Collections;
using NetPortals;

namespace Network
{
    public class GameManager : NetworkBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject gameUIObject;
        [SerializeField] private GameObject scoreboardUIPanel;
        

        [Header("Game Systems References")]
        [SerializeField] private GameObject playerSpawnerObject;
        [SerializeField] private GameObject playerManagerPrefab;
        [SerializeField] int numOfKillsToWin;

        private NetworkList<PlayerState> playerStates = new NetworkList<PlayerState>();
        private NetworkVariable<GameInfo> gameInfo = new NetworkVariable<GameInfo>();
        
        private ScoreboardUI scoreboardUI;
        private PlayerScoreUI playerScoreUI;
        private GameScoreUI gameScoreUI;
        private EndGameUI endGameUI;
        private GameObject gameInfoObject;
        
        public void Start() {
            //Assigning UI References to proper elements
            scoreboardUI = gameUIObject.GetComponent<ScoreboardUI>();
            playerScoreUI = gameUIObject.GetComponent<PlayerScoreUI>();
            gameScoreUI = gameUIObject.GetComponent<GameScoreUI>();
            endGameUI = gameUIObject.GetComponent<EndGameUI>();

            scoreboardUIPanel.SetActive(false);
            endGameUI.SetGameobjectActive(false);

            if (IsClient)
            {
                playerStates.OnListChanged += HandlePlayerStateChange;
                gameInfo.OnValueChanged += HandleGameInfoChange;
                
                UpdateGameMode();
                ScoreboardUpdate();
                PlayerScoreUpdate();
                GameScoreUpdate();
            }
            if(IsServer)
            {
                //Game Info
                gameInfoObject = GameObject.FindGameObjectWithTag("GameInfoManager");
                gameInfo.Value = gameInfoObject.GetComponent<GameInfoManager>().GetGameInfo();

                //Check game state
                playerStates.OnListChanged += CheckEndGameState;

                playerSpawnerObject.GetComponent<PlayerSpawner>()
                .UpdateGameInfo(gameInfo.Value);

                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

                foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientLoaded(client.ClientId);
                }
            }
        }

        private void Update() {
            
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                scoreboardUIPanel.SetActive(true);
            }
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                scoreboardUIPanel.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            playerStates.OnListChanged -= HandlePlayerStateChange;
            gameInfo.OnValueChanged -= HandleGameInfoChange;

            if(IsServer) playerStates.OnListChanged -= CheckEndGameState;

            if(NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        #region Connections

        private void HandleClientConnected(ulong clientId) {
            var tmpGameInfo = gameInfo.Value;
            tmpGameInfo.playerCount++;
            tmpGameInfo.maxPlayersPerTeam++;
            gameInfo.Value = tmpGameInfo;
            
            playerSpawnerObject.GetComponent<PlayerSpawner>()
                .UpdateGameInfo(gameInfo.Value);
            HandleClientLoaded(clientId);
        }

        private void HandleClientLoaded(ulong clientId) {
            PlayerManager playerManager = null;
            try {
                playerManager = NetworkManager.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerManager>();
                if (!playerManager)
                    throw new Exception();
            }
            catch {
                //No player manager found
                //We create a new one
                playerManager = CreatePlayerManager(clientId);
            }
            
            playerSpawnerObject.GetComponent<PlayerSpawner>().SpawnPlayer(playerManager);
            Debug.Log("Player with id " + clientId + " has joined the game!");
            playerStates.Add(playerManager.GetPlayerState());
            playerManager.DestoryPlayerManager();
        }

        private PlayerManager CreatePlayerManager(ulong clientId) {
            var newPlayerManagerObject = Instantiate(playerManagerPrefab);

            if(!newPlayerManagerObject)
                throw new Exception();

            newPlayerManagerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            
            var newPlayerManager = newPlayerManagerObject.GetComponent<PlayerManager>();
            
            var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);
            
            int teamId = 0;

            if(gameInfo.Value.gameMode == GameMode.TeamDeathmatch)
            {
                teamId = AssignTeamToNewPlayer();
            }

            newPlayerManager.SetPlayerData(clientId, playerData.Value.PlayerName, teamId);

            return newPlayerManager;
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            var tmpGameInfo = gameInfo.Value;
            tmpGameInfo.playerCount--;
            tmpGameInfo.maxPlayersPerTeam--;
            gameInfo.Value = tmpGameInfo;
            
            for(int i = 0; i < playerStates.Count; i++)
            {
                if(playerStates[i].ClientId == clientId)
                {
                    Debug.Log("Player with id " + clientId + " has left the game.");  
                    playerStates.RemoveAt(i);
                }
            }
            playerSpawnerObject.GetComponent<PlayerSpawner>()
                .UpdateGameInfo(gameInfo.Value);
        }

        #endregion Connections

        #region Game State Changes

        private void HandlePlayerStateChange(NetworkListEvent<PlayerState> state)
        {
            //All UI Updates
            ScoreboardUpdate();
            PlayerScoreUpdate();
            GameScoreUpdate();
        }

        private void HandleGameInfoChange(GameInfo prevInfo, GameInfo newInfo)
        {
            UpdateGameMode();
        }

        private void UpdateGameMode()
        {
            scoreboardUI.UpdateGameMode(gameInfo.Value.gameMode);
            gameScoreUI.UpdateGameMode(gameInfo.Value.gameMode);
        }

        public void PlayerKillUpdate(ulong clientId)
        {
            PlayerKillServerRpc(clientId);
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

        public void PlayerDeathUpdate(ulong clientId)
        {
            PlayerDeathServerRpc(clientId);
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

        private void CheckEndGameState(NetworkListEvent<PlayerState> state)
        {
            for (int i = 0; i < playerStates.Count; i++)
            {
                if (playerStates[i].PlayerKills >= numOfKillsToWin)
                {
                    GameEndedClientRpc(playerStates[i].ClientId, 0);
                    StartCoroutine(ServerEndGameCountdown());
                    break;
                }
            } 
        }
        
        [ClientRpc]
        private void GameEndedClientRpc(ulong clientId, int teamId, ClientRpcParams rpcParams = default)
        {
            endGameUI.SetGameobjectActive(true);
            bool winStatus = false;
            if(teamId == 0)
            {   
                if(clientId == NetworkManager.Singleton.LocalClientId) 
                    winStatus = true;
            }else
            {
                if(playerStates.Where(
                    p => p.ClientId == NetworkManager.Singleton.LocalClientId)
                    .FirstOrDefault().TeamId == teamId)
                    {
                        winStatus = true;
                    }
            }   
            endGameUI.UpdateEndGameText(winStatus);
            StartCoroutine(ClientEndGameCountdown());
        }

        private IEnumerator ServerEndGameCountdown()
        {
            yield return new WaitForSeconds(5);
            ServerGameNetPortal.Instance.EndRound();
        }
        
        private IEnumerator ClientEndGameCountdown()
        {
            yield return new WaitForSeconds(5);
            Cursor.lockState = CursorLockMode.None;
        }

        #endregion Game State Changes

        #region UI Updates

        private void GameScoreUpdate()
        {
            gameScoreUI.DeleteScores();
            if(gameInfo.Value.gameMode == GameMode.TeamDeathmatch)
            {
                for(int i = 0; i < gameInfo.Value.teamCount; i++)
                {
                    gameScoreUI.AddTeamScores(i, GetTeamScore(i + 1));
                }
            } else if(gameInfo.Value.gameMode == GameMode.FreeForAll)
            {
                PlayerState localPlayerState = GetLocalPlayerState();
                PlayerState topPlayer = GetTopPlayerScore();
                gameScoreUI.SetPlayerScore(topPlayer, 0);
                gameScoreUI.SetPlayerScore(localPlayerState, 1);
            }   
        }
        
        private void ScoreboardUpdate()
        {
            scoreboardUI.DestroyCards();
            
            if(gameInfo.Value.gameMode == GameMode.FreeForAll)
            {
                int i = 0;
                foreach(var player in playerStates)
                {
                    float position = -(30*i + 30*(i+1));
                    scoreboardUI.CreateListItem(player, position,
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
                            scoreboardUI.CreateListItem(player, position,
                            NetworkManager.Singleton.LocalClientId == player.ClientId);
                            k++;
                        }
                    }
                }
            }
        }

        private void PlayerScoreUpdate()
        {
            foreach(var playerState in playerStates)
            {
                if(NetworkManager.Singleton.LocalClientId == playerState.ClientId)
                {
                    playerScoreUI.UpdatePlayerScore(playerState);
                    break;
                }
            }
        }

        #endregion UI Updates

        #region Utility

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

        public PlayerState GetLocalPlayerState()
        {
            foreach(var playerState in playerStates)
            {
                if(playerState.ClientId == NetworkManager.Singleton.LocalClientId) return playerState;
            }
            return new PlayerState();
        }

        public PlayerState GetTopPlayerScore()
        {
            PlayerState topPlayer = new PlayerState();
            foreach(var playerState in playerStates)
            {
                if(playerState.PlayerKills >= topPlayer.PlayerKills)
                {
                    topPlayer = playerState;
                }
            }
            return topPlayer;
        }

        public int AssignTeamToNewPlayer()
        {
            int teamCount = gameInfo.Value.teamCount;
            int[] teamsCount = new int[teamCount];
            for(int i = 0; i < teamCount; i++)
            {
                teamsCount[i] = PlayersPerTeam(i+1);
            }
            return Array.IndexOf(teamsCount, teamsCount.Min()) + 1;
        }

        public int PlayersPerTeam(int teamId)
        {
            int value = 0;
            foreach(var playerState in playerStates)
            {
                if(playerState.TeamId == teamId) value++;
            }
            return value;
        }

        #endregion Utility
    }
}
