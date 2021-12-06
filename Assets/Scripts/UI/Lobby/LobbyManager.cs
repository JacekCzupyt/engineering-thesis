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
    [SerializeField] private GameObject playerManager;
    [SerializeField] private GameObject playerCardPrefab;
    [SerializeField] private GameObject playerCardPanel;
    [SerializeField] private GameObject playerCountText;
    private NetworkList<LobbyPlayerState> lobbyPlayers = new NetworkList<LobbyPlayerState>();
    private List<LobbyPlayerCard> playerCards = new List<LobbyPlayerCard>();

    private bool IsPlayerReadyUI;

    public override void NetworkStart()
    {
        IsPlayerReadyUI = false;
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
        ToggleReadyUpButtonText();
    }

    private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
    {
        DestroyCards();
        for(int i = 0; i < lobbyPlayers.Count; i++)
        {
            float position = -(30*i + 20*(i+1));
            InitializePlayerCard(lobbyPlayers[i], position);
        }
        UpdatePlayerCount();
        
        if(IsHost)
        {
            startGameButton.interactable = IsEveryoneReady();
        }
    }

    private void ToggleReadyUpButtonText()
    {
        if(IsPlayerReadyUI)
        {
            readyUpButton.GetComponentInChildren<Text>().text =  "Ready Up!";
            IsPlayerReadyUI = false;
        }
        else
        {
            readyUpButton.GetComponentInChildren<Text>().text =  "Cancel";
            IsPlayerReadyUI = true;
        }
    }

    private void InitializePlayerCard(LobbyPlayerState state, float position)
    {
        GameObject obj = Instantiate(playerCardPrefab, new Vector3(0, position, 0), Quaternion.identity) as GameObject;
        LobbyPlayerCard card = new LobbyPlayerCard(obj, state);
        card.SetParent(playerCardPanel);
        card.SetActive(true);
        playerCards.Add(card);
    }

    private void DestroyCards()
    {
        foreach(LobbyPlayerCard card in playerCards)
        {
            Destroy(card.playerCard);
        }
        playerCards.Clear();
    }

    private void UpdatePlayerCount()
    {
        playerCountText.GetComponent<Text>().text = "Players("+lobbyPlayers.Count+"/10)";
    }

}

