using MLAPI;
using MLAPI.Connection;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;
using UI.Game;
using UnityEngine;

namespace Network {
    public class ScoreboardManager : NetworkBehaviour
    {
        [SerializeField] private GameObject scoreboardUIObject;
        private NetworkList<ScorePlayerState> scoreboardPlayers = new NetworkList<ScorePlayerState>();
        private PlayerScoreUI playerScoreUI;
        public void Start() {
            playerScoreUI = scoreboardUIObject.GetComponent<PlayerScoreUI>();
            playerScoreUI.gameObject.SetActive(false);                 
            if(IsClient)
            {
                scoreboardPlayers.OnListChanged += HandlePlayerScoreboardStateChange;
                if(scoreboardPlayers.Count > 0)
                {
                    HandlePlayerScoreboardStateChange(new NetworkListEvent<ScorePlayerState>());
                }
            }
            if(IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback  += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback  += HandleClientDisconnect;
                foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                }
            } 
        }

        private void OnDestroy() {
            scoreboardPlayers.OnListChanged -= HandlePlayerScoreboardStateChange;

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

            if(!playerManager) return;
            scoreboardPlayers.Add(playerManager.ToPlayerScoreState());
        }
    
        private void HandleClientDisconnect(ulong clientId)
        {
            for(int i = 0; i < scoreboardPlayers.Count; i++)
            {
                if(scoreboardPlayers[i].ClientId == clientId)
                {
                    scoreboardPlayers.RemoveAt(i);
                    break;
                }
            }
        }
    
        [ServerRpc(RequireOwnership = false)]
        private void PlayerKillServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
        {
            for(int i = 0; i < scoreboardPlayers.Count; i++)
            {
                if(scoreboardPlayers[i].ClientId == clientId)
                {
                    scoreboardPlayers[i] = new ScorePlayerState(
                        scoreboardPlayers[i].ClientId,
                        scoreboardPlayers[i].PlayerName,
                        scoreboardPlayers[i].PlayerKills + 1,
                        scoreboardPlayers[i].PlayerDeaths
                    );
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayerDeathServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
        {
            for(int i = 0; i < scoreboardPlayers.Count; i++)
            {
                if(scoreboardPlayers[i].ClientId == clientId)
                {
                    scoreboardPlayers[i] = new ScorePlayerState(
                        scoreboardPlayers[i].ClientId,
                        scoreboardPlayers[i].PlayerName,
                        scoreboardPlayers[i].PlayerKills,
                        scoreboardPlayers[i].PlayerDeaths + 1
                    );
                }
            }
        }

        private void HandlePlayerScoreboardStateChange(NetworkListEvent<ScorePlayerState> scoreboardState)
        {
            playerScoreUI.DestroyCards();
            for(int i = 0; i < scoreboardPlayers.Count; i++)
            {
                float position = -(30*i + 30*(i+1));
                playerScoreUI.CreateListItem(scoreboardPlayers[i], position);
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

        public void PlayerKillUpdate(ulong clientId)
        {
            PlayerKillServerRpc(clientId);
        }

        public void PlayerDeathUpdate(ulong clientId)
        {
            PlayerDeathServerRpc(clientId);
        }
    }
}
