using System;
using System.Collections.Generic;
using Game_Systems.Utility;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;
using NetPortals;
using UI.Lobby;
using UnityEngine;

namespace Network {
    public class LobbyManager : NetworkBehaviour
    {
        [Header("UI Object References")]
        [SerializeField] private GameObject lobbyUIObject;

        [Header("Prefab References")]
        [SerializeField] private GameObject playerManagerPrefab;
        [SerializeField] private GameObject gameInfoManagerPrefab;

        [Header("Game Settings")]
        [SerializeField] private int numberOfTeams = 2;
        private NetworkDictionary<ulong, LobbyPlayerState> lobbyPlayers = new NetworkDictionary<ulong, LobbyPlayerState>();
        public NetworkVariable<GameMode> gameMode = new NetworkVariable<GameMode>(GameMode.FreeForAll);
        private NetworkVariable<bool> arrangeCards = new NetworkVariable<bool>(false);
        private LobbyUI lobbyUI;

        private void Start() {
            Cursor.lockState = CursorLockMode.None;
        }
        public override void NetworkStart()
        {
            lobbyUI = lobbyUIObject.GetComponent<LobbyUI>();
            if(IsClient)
            {
                lobbyPlayers.OnDictionaryChanged += HandleLobbyPlayersStateChanged;
                gameMode.OnValueChanged += HandleGameModeChange;      
                UpdateGameMode();
            }
        
            if(IsServer)
            {
                lobbyUI.startGameButton.gameObject.SetActive(true);
                lobbyUI.changeModeButton.gameObject.SetActive(true);

                NetworkManager.Singleton.OnClientConnectedCallback  += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback  += HandleClientDisconnect;

                foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                }
            }
        }

        private void OnDestroy() {
            lobbyPlayers.OnDictionaryChanged -= HandleLobbyPlayersStateChanged;
            gameMode.OnValueChanged -= HandleGameModeChange;

            if(NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        private bool IsEveryoneReady()
        {
            foreach(var player in lobbyPlayers.Values)
            {
                if(!player.IsReady) return false;
            }
            return true;
        }

        private void HandleClientConnected(ulong clientId)
        {
            var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

            if(!playerData.HasValue) return;

            lobbyPlayers[clientId] = new LobbyPlayerState(
                clientId,
                playerData.Value.PlayerName,
                0,
                false
            );
        }

        private void HandleClientDisconnect(ulong clientId) {
            if (!lobbyPlayers.Remove(clientId))
                throw new InvalidOperationException("Can't disconnect non-existent client");
        }
    
        [ServerRpc(RequireOwnership = false)]
        private void SpawnPlayerManagerServerRpc(ulong clientId, ServerRpcParams serverParams = default) {
            var manager = Instantiate(playerManagerPrefab);
            manager.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            LobbyPlayerState playerState = lobbyPlayers[clientId];
            manager.GetComponent<PlayerManager>().SetPlayerData(clientId, 
                playerState.PlayerName,
                playerState.TeamId
                );
        }

        [ServerRpc(RequireOwnership = false)]
        private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default) {
            var playerState = lobbyPlayers[serverRpcParams.Receive.SenderClientId];
            playerState.IsReady = !playerState.IsReady;
            lobbyPlayers[playerState.ClientId] = playerState;
        }

        [ServerRpc(RequireOwnership = false)]
        private void StartGameServerRpc(ServerRpcParams serverRpcParams = default)
        {
            if(serverRpcParams.Receive.SenderClientId != NetworkManager.Singleton.LocalClientId) return;

            if(!IsEveryoneReady()) return;

            if(gameMode.Value == GameMode.TeamDeathmatch){
                
                if(lobbyPlayers.Count >= numberOfTeams)
                {
                    RandomizeTeams(lobbyPlayers.Count, numberOfTeams);
                }
                else{
                    Debug.Log("Number of teams cannot be more than the number of players");
                    return;
                }
            }

            foreach(var player in lobbyPlayers.Values)
            {
                SpawnPlayerManagerServerRpc(player.ClientId);
            }

            InitializeGameInfoObject();

            ServerGameNetPortal.Instance.StartGame();
        }

        private void RandomizeTeams(int playersCount, int numOfTeams){
            int[] playersPerTeam = new int[numberOfTeams];
            for(int i = 0; i < numberOfTeams; i++)
            {
                playersPerTeam[i] = (int) Math.Ceiling((double) playersCount/(double) numberOfTeams);
            }

            List<LobbyPlayerState> tempStates = new List<LobbyPlayerState>(lobbyPlayers.Values);

            foreach(var player in tempStates)
            {
                int teamId = RespawnPointGenerator.rnd.Next(numberOfTeams);
                while(playersPerTeam[teamId] == 0)
                {
                    teamId = RespawnPointGenerator.rnd.Next(numberOfTeams);
                }

                lobbyPlayers[player.ClientId] = new LobbyPlayerState(
                    player.ClientId,
                    player.PlayerName,
                    teamId + 1,
                    player.IsReady
                );
            
                playersPerTeam[teamId]--;
            }
            // var ids = lobbyPlayers.Keys;
            // int index = 0;
            // foreach(var id in ids) {
            //     var playerState = lobbyPlayers[id];
            //     playerState.TeamId = (index % numOfTeams) + 1;
            //     lobbyPlayers[id] = playerState;
            //     index++;
            // }
        }

        private void InitializeGameInfoObject()
        {
            foreach(var manager in GameObject.FindGameObjectsWithTag("GameInfoManager")) {
                Destroy(manager);
            }
            
            var gameInfo = Instantiate(gameInfoManagerPrefab);
            gameInfo.GetComponent<GameInfoManager>().SetGameInfo(new GameInfo(gameMode.Value, numberOfTeams, lobbyPlayers.Count, lobbyPlayers.Count)); 
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeGameModeServerRpc(GameMode mode, ServerRpcParams serverRpcParams = default){
            gameMode.Value = mode;
        }

        [ServerRpc(RequireOwnership = false)]
        private void ToggleArrangeCardsServerRpc(ServerRpcParams serverRpcParams = default){
            arrangeCards.Value = !arrangeCards.Value;
        }

        public void LeaveGame()
        {
            GameNetPortal.Instance.RequestDisconnect();
        }

        public void StartGame(){
            ToggleArrangeCardsServerRpc();
            StartGameServerRpc();
        }

        public void ReadyUp(){
            ToggleReadyServerRpc();
        }

        public void SetGameMode(GameMode mode){
            ChangeGameModeServerRpc(mode);
        }

        private void HandleLobbyPlayersStateChanged(NetworkDictionaryEvent<ulong, LobbyPlayerState> lobbyState)
        {
            lobbyUI.DestroyCards();
            if(gameMode.Value == GameMode.TeamDeathmatch && arrangeCards.Value == true)
            {
                int k = 0;
                for(int i = 1; i < numberOfTeams + 1; i++) {
                    foreach(var playerState in lobbyPlayers.Values)
                    {
                        if(playerState.TeamId == i){
                            float position = -(30*k + 20*(k+1));
                            lobbyUI.CreateListItem(playerState, position);
                            k++;
                        }
                    }
                }
            }else {
                int k = 0;
                foreach(var playerState in lobbyPlayers.Values) {
                    float position = -(30*k + 20*(k+1));
                    lobbyUI.CreateListItem(playerState, position);
                    k++;
                }
            }
            UpdatePlayerCount();
        
            if(IsHost)
            {
                lobbyUI.startGameButton.interactable = IsEveryoneReady();
            }
        }

        private void HandleGameModeChange(GameMode prevMode, GameMode newMode){
            UpdateGameMode();
        }

        private void UpdatePlayerCount()
        {
            lobbyUI.UpdatePlayerCount(lobbyPlayers.Count);
        }

        private void UpdateGameMode(){
            lobbyUI.UpdateGameMode(gameMode.Value);
        }
    }
}

