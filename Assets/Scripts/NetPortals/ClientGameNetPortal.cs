using System;
using System.Text;
using MLAPI;
using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI.Transports.UNET;

namespace NetPortals {
    [RequireComponent(typeof(GameNetPortal))]
    public class ClientGameNetPortal : MonoBehaviour {
        public static ClientGameNetPortal Instance => instance;
        private static ClientGameNetPortal instance;

        public DisconnectReason DisconnectReason { get; private set; } = new DisconnectReason();

        public event Action<ConnectStatus> OnConnectionFinished;

        public event Action OnNetworkTimedOut;

        private GameNetPortal gameNetPortal;

        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            gameNetPortal = GetComponent<GameNetPortal>();

            gameNetPortal.OnNetworkReadied += HandleNetworkReadied;
            gameNetPortal.OnConnectionFinished += HandleConnectionFinished;
            gameNetPortal.OnDisconnectReasonReceived += HandleDisconnectReasonReceived;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        private void OnDestroy() {
            if (gameNetPortal == null) {
                return;
            }

            gameNetPortal.OnNetworkReadied -= HandleNetworkReadied;
            gameNetPortal.OnConnectionFinished -= HandleConnectionFinished;
            gameNetPortal.OnDisconnectReasonReceived -= HandleDisconnectReasonReceived;

            if (NetworkManager.Singleton == null) {
                return;
            }

            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }

        public void StartClient() {
            
            if (NetworkManager.Singleton.IsClient)
                NetworkManager.Singleton.StopClient();
            
            if (NetworkManager.Singleton.IsHost)
                NetworkManager.Singleton.StopHost();
            
            
            var payload = JsonUtility.ToJson(
                new ConnectionPayload() {
                    clientGUID = Guid.NewGuid().ToString(),
                    clientScene = SceneManager.GetActiveScene().buildIndex,
                    playerName = PlayerPrefs.GetString("PlayerName", "Missing Name")
                }
            );

            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

            NetworkManager.Singleton.StartClient();
        }

        private void HandleNetworkReadied() {
            if (!NetworkManager.Singleton.IsClient) {
                return;
            }

            if (!NetworkManager.Singleton.IsHost) {
                gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
            }

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode) {
            gameNetPortal.ClientToServerSceneChanged(SceneManager.GetActiveScene().buildIndex);
        }

        private void HandleUserDisconnectRequested() {
            DisconnectReason.SetDisconnectReason(ConnectStatus.UserRequestedDisconnect);
            NetworkManager.Singleton.StopClient();

            HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);

            SceneManager.LoadScene("MenuScene");
        }

        private void HandleConnectionFinished(ConnectStatus status) {
            if (status != ConnectStatus.Success) {
                DisconnectReason.SetDisconnectReason(status);
            }

            OnConnectionFinished?.Invoke(status);
        }

        private void HandleDisconnectReasonReceived(ConnectStatus status) {
            DisconnectReason.SetDisconnectReason(status);
        }

        private void HandleClientDisconnect(ulong clientId) {
            if (!NetworkManager.Singleton.IsConnectedClient && !NetworkManager.Singleton.IsHost) {
                SceneManager.sceneLoaded -= HandleSceneLoaded;
                gameNetPortal.OnUserDisconnectRequested -= HandleUserDisconnectRequested;

                if (SceneManager.GetActiveScene().name != "MenuScene") {
                    if (!DisconnectReason.HasTransitionReason) {
                        DisconnectReason.SetDisconnectReason(ConnectStatus.GenericDisconnect);
                    }

                    SceneManager.LoadScene("MenuScene");
                }
                else {
                    OnNetworkTimedOut?.Invoke();
                }
            }
        }

        public void SetConnectAddress(string address) {
            UNetTransport transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
            if (address.Length > 0) {
                transport.ConnectAddress = address;
                Debug.Log(transport.ConnectAddress);
            }
            else {
                transport.ConnectAddress = "127.0.0.1";
            }
        }
    }
}
