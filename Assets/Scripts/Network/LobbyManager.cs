using System.Linq;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable.Collections;
using MLAPI.Connection;
using MLAPI.Messaging;
using Network;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private GameObject lobbyUIObject; 
    [SerializeField] private GameObject playerManager;
    private NetworkList<LobbyPlayerState> lobbyPlayers = new NetworkList<LobbyPlayerState>();
    private LobbyUI lobbyUI;
    public override void NetworkStart()
    {
        lobbyUI = lobbyUIObject.GetComponent<LobbyUI>();
        if(IsClient)
        {
            lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;            
            SpawnPlayerManagerServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        
        if(IsServer)
        {
            lobbyUI.startGameButton.gameObject.SetActive(true);
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

    private void UpdatePlayerCount()
    {
        lobbyUI.UpdatePlayerCount(lobbyPlayers.Count);
    }
}

