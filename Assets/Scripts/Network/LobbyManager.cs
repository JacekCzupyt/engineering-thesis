using System.Linq;
using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using MLAPI.NetworkVariable;
using NetPortals;
using UI.Lobby;
using UnityEngine;

namespace Network {
    public class LobbyManager : NetworkBehaviour
    {
        [SerializeField] private GameObject lobbyUIObject; 
        [SerializeField] private GameObject playerManager;
        private NetworkList<LobbyPlayerState> lobbyPlayers = new NetworkList<LobbyPlayerState>();
        private NetworkVariable<GameMode> gameMode = new NetworkVariable<GameMode>(GameMode.FreeForAll);
        private LobbyUI lobbyUI;
        public override void NetworkStart()
        {
            lobbyUI = lobbyUIObject.GetComponent<LobbyUI>();

            if(IsClient)
            {
                lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
                gameMode.OnValueChanged += HandleGameModeChange;      
                SpawnPlayerManagerServerRpc(NetworkManager.Singleton.LocalClientId);
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
            var manager = Instantiate(playerManager);
            manager.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            manager.GetComponent<PlayerManager>().SetPlayerData(clientId, 
                lobbyPlayers.Where(p => p.ClientId == clientId).FirstOrDefault().PlayerName);
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

            ServerGameNetPortal.Instance.StartGame();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeGameModeServerRpc(GameMode mode, ServerRpcParams serverRpcParams = default){
            gameMode.Value = mode;
        }

        public void LeaveGame()
        {
            GameNetPortal.Instance.RequestDisconnect();
        }

        public void StartGame(){
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
            for(int i = 0; i < lobbyPlayers.Count; i++)
            {
                float position = -(30*i + 20*(i+1));
                lobbyUI.CreateListItem(lobbyPlayers[i], position);
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

