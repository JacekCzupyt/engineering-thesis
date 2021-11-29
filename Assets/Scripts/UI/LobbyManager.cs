using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable.Collections;
using MLAPI.Connection;
using MLAPI.Messaging;

public class LobbyManager : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Button readyUpButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private GameObject ListItemPrefab;
    [SerializeField] private GameObject listItemsPanel;
    private NetworkList<LobbyPlayerState> lobbyPlayers = new NetworkList<LobbyPlayerState>();
    private List<GameObject> playerItems = new List<GameObject>();

    [SerializeField] private GameObject playerManager;

    public override void NetworkStart()
    {
        InitializePlayerItems();
        if(IsClient)
        {
            lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
            SpawnPlayerManagerServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        
        if(IsServer)
        {
            startGameButton.gameObject.SetActive(true);

            NetworkManager.Singleton.OnClientConnectedCallback  += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback  += HandleClientDisconnect;

            foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }

    }

    private void InitializePlayerItems()
    {
        for(int i = 0; i < 10; i++)
        {
            int position = -(30*i + 20*(i+1));
            GameObject listItem = Instantiate(ListItemPrefab, new Vector3(0, position, 0), Quaternion.identity) as GameObject;
            listItem.transform.SetParent(listItemsPanel.transform, false);
            listItem.SetActive(false);
            playerItems.Add(listItem);
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

    public void OnLeaveButtonClicked()
    {
        GameNetPortal.Instance.RequestDisconnect();
    }

    public void OnStartGameButtonClicked(){
        StartGameServerRpc();
    }

    public void OnReadyUpButtonClicked(){
        ToggleReadyServerRpc();
    }

    private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
    {
        ClearItemListPanel();
        for(int i = 0; i < lobbyPlayers.Count; i++)
        {
            UpdateListItem(playerItems[i], lobbyPlayers[i]);
        }

        if(IsHost)
        {
            startGameButton.interactable = IsEveryoneReady();
        }
    }

    private void UpdateListItem(GameObject listItem, LobbyPlayerState lobbyPlayerState)
    {
        Text playerTextbox = listItem.transform.GetChild(0).gameObject.GetComponent<Text>();
        playerTextbox.text = lobbyPlayerState.PlayerName;
        Image playerIsReadyIndicator = listItem.transform.GetChild(1).gameObject.GetComponent<Image>();
        if(lobbyPlayerState.IsReady)
        {
            playerIsReadyIndicator.color = Color.green;
        }else
        {
            playerIsReadyIndicator.color = Color.red;
        }
        listItem.SetActive(true);
    }

    private void ClearItemListPanel()
    {
        foreach(var item in playerItems)
        {
            item.SetActive(false);
        }
    }
}
