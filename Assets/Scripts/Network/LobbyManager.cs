using System;
using System.Linq;
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
        private NetworkList<LobbyPlayerState> lobbyPlayers = new NetworkList<LobbyPlayerState>();
        private NetworkVariable<GameMode> gameMode = new NetworkVariable<GameMode>(GameMode.FreeForAll);
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
                lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
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
            lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;
            gameMode.OnValueChanged -= HandleGameModeChange;

            if(NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        private bool IsEveryoneReady()
        {
            foreach(var player in lobbyPlayers)
            {
                if(!player.IsReady) return false;
            }
            return true;
        }

        private void HandleClientConnected(ulong clientId)
        {
            var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

            if(!playerData.HasValue) return;

            lobbyPlayers.Add(new LobbyPlayerState(
                clientId,
                playerData.Value.PlayerName,
                0,
                false
            ));
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            for(int i = 0; i < lobbyPlayers.Count; i++)
            {
                if(lobbyPlayers[i].ClientId == clientId)
                {
                    lobbyPlayers.RemoveAt(i);
                    break;
                }
            }
        }
    
        [ServerRpc(RequireOwnership = false)]
        private void SpawnPlayerManagerServerRpc(ulong clientId, ServerRpcParams serverParams = default) {
            var manager = Instantiate(playerManagerPrefab);
            manager.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            LobbyPlayerState playerState = lobbyPlayers.Where(p => p.ClientId == clientId).FirstOrDefault();
            manager.GetComponent<PlayerManager>().SetPlayerData(clientId, 
                playerState.PlayerName,
                playerState.TeamId
                );
        }

        [ServerRpc(RequireOwnership = false)]
        private void ToggleReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            for(int i = 0; i < lobbyPlayers.Count; i++)
            {
                if(lobbyPlayers[i].ClientId == serverRpcParams.Receive.SenderClientId)
                {
                    lobbyPlayers[i] = new LobbyPlayerState(
                        lobbyPlayers[i].ClientId,
                        lobbyPlayers[i].PlayerName,
                        lobbyPlayers[i].TeamId,
                        !lobbyPlayers[i].IsReady
                    );
                }
            }
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

            foreach(var player in lobbyPlayers)
            {
                SpawnPlayerManagerServerRpc(player.ClientId);
            }

            InitializeGameInfoObject();

            ServerGameNetPortal.Instance.StartGame();
        }

        private void RandomizeTeams(int playersCount, int numOfTeams){
            for(int i = 0, j = 0; i < playersCount; i++){
                int teamNo = j + 1;
                lobbyPlayers[i] = new LobbyPlayerState(
                    lobbyPlayers[i].ClientId,
                    lobbyPlayers[i].PlayerName,
                    teamNo,
                    lobbyPlayers[i].IsReady
                );
                if(j < (numOfTeams - 1)) j++;
                else j = 0;
            }
        }

        private void InitializeGameInfoObject()
        {
            var gameInfo = Instantiate(gameInfoManagerPrefab);
            gameInfo.GetComponent<GameInfoManager>().SetGameInfo(new GameInfo(gameMode.Value, numberOfTeams, lobbyPlayers.Count)); 
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

        private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
        {
            lobbyUI.DestroyCards();
            if(gameMode.Value == GameMode.TeamDeathmatch && arrangeCards.Value == true)
            {
                int k = 0;
                for(int i = 1; i < numberOfTeams + 1; i++)
                {
                    for(int j = 0; j < lobbyPlayers.Count; j++)
                    {
                        if(lobbyPlayers[j].TeamId == i){
                            float position = -(30*k + 20*(k+1));
                            lobbyUI.CreateListItem(lobbyPlayers[j], position);
                            k++;
                        }
                    }
                }
            }else
            {
                for(int i = 0; i < lobbyPlayers.Count; i++)
                {
                    float position = -(30*i + 20*(i+1));
                    lobbyUI.CreateListItem(lobbyPlayers[i], position);
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

