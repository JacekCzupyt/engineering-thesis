using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MLAPI;
using MLAPI.SceneManagement;
using MLAPI.Spawning;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UI.MainMenu;

namespace NetPortals {
    public class ServerGameNetPortal : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int maxPlayers = 10;

        public static ServerGameNetPortal Instance => instance;
        private static ServerGameNetPortal instance;
        
        private Dictionary<string, PlayerData> clientData;
        private Dictionary<ulong, string> clientIdToGuid;
        private Dictionary<ulong, int> clientSceneMap;
        private bool gameInProgress;

        private const int MaxConnectionPayload = 1024;

        private GameNetPortal gameNetPortal;

        private string uri = "http://79.191.52.229:8080/servers";
        private int DbId;

        private int counter = 0;
        [SerializeField] AddServerManager servert;
        bool isAdd;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            
            gameNetPortal = GetComponent<GameNetPortal>();
            gameNetPortal.OnNetworkReadied += HandleNetworkReadied;

            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;

            clientData = new Dictionary<string, PlayerData>();
            clientIdToGuid = new Dictionary<ulong, string>();
            clientSceneMap = new Dictionary<ulong, int>();
        }

        private void OnDestroy()
        {
            if (gameNetPortal == null) { return; }

            gameNetPortal.OnNetworkReadied -= HandleNetworkReadied;

            if (NetworkManager.Singleton == null) { return; }

            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
            
        }

        public PlayerData? GetPlayerData(ulong clientId)
        {
            if (clientIdToGuid.TryGetValue(clientId, out string clientGuid))
            {
                if (clientData.TryGetValue(clientGuid, out PlayerData playerData))
                {
                    return playerData;
                }
                else
                {
                    Debug.LogWarning($"No player data found for client id: {clientId}");
                }
            }
            else
            {
                Debug.LogWarning($"No client guid found for client id: {clientId}");
            }

            return null;
        }

        public void StartGame()
        {
            gameInProgress = true;

            NetworkSceneManager.SwitchScene("GameScene");
        }

        public void EndRound()
        {
            gameInProgress = false;

            NetworkSceneManager.SwitchScene("LobbyScene");
        }

        private void HandleNetworkReadied()
        {
            if (!NetworkManager.Singleton.IsServer) { return; }

            gameNetPortal.OnUserDisconnectRequested += HandleUserDisconnectRequested;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
            gameNetPortal.OnClientSceneChanged += HandleClientSceneChanged;

            NetworkSceneManager.SwitchScene("LobbyScene");

            if (NetworkManager.Singleton.IsHost)
            {
                clientSceneMap[NetworkManager.Singleton.LocalClientId] = SceneManager.GetActiveScene().buildIndex;
            }
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            clientSceneMap.Remove(clientId);

            if (clientIdToGuid.TryGetValue(clientId, out string guid))
            {
                clientIdToGuid.Remove(clientId);

                if (clientData[guid].ClientId == clientId)
                {
                    clientData.Remove(guid);
                }
            }

            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                gameNetPortal.OnUserDisconnectRequested -= HandleUserDisconnectRequested;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
                gameNetPortal.OnClientSceneChanged -= HandleClientSceneChanged;
            }
        }

        private void HandleClientSceneChanged(ulong clientId, int sceneIndex)
        {
            clientSceneMap[clientId] = sceneIndex;
        }

        private void HandleUserDisconnectRequested()
        {
            if (isAdd)
            {
                CancelInvoke("Upload");               
            }
            StartCoroutine(DelServer());
            HandleClientDisconnect(NetworkManager.Singleton.LocalClientId);

            NetworkManager.Singleton.StopHost();

            ClearData();

            SceneManager.LoadScene("MenuScene");
        }
        private IEnumerator DelServer()
        {
            using (UnityWebRequest www = UnityWebRequest.Delete(uri + $"/{DbId}"))
            {
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Success");
                }
            }
        }
            
        private void HandleServerStarted()
        {
            
            if (!NetworkManager.Singleton.IsHost) { return; }
            isAdd = servert.IsAdd;
            if (isAdd)
            {
                string jsonData = "{\"name\": \"" + servert.serverNameInput.text + "\", \"ip\": \"" + servert.ip + "\"}";
                StartCoroutine(AddServer(uri, jsonData));
                InvokeRepeating("Upload", 5.0f, 8.0f);
            }            
            string clientGuid = Guid.NewGuid().ToString();
            string playerName = PlayerPrefs.GetString("PlayerName", "Missing Name");
            clientData.Add(clientGuid, new PlayerData(playerName, NetworkManager.Singleton.LocalClientId));
            clientIdToGuid.Add(NetworkManager.Singleton.LocalClientId, clientGuid);
        }
        private IEnumerator AddServer(string uri, string data)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(uri, ""))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    CancelInvoke("Upload");
                    Debug.Log(www.error);
                    
                }
                else
                {
                    Dictionary<string, string> hdrs = www.GetResponseHeaders();
                    Debug.Log("Success");
                    string[] k = hdrs["Location"].Split('/');
                    Int32.TryParse(k[2], out DbId);
                }
            }
        }
        
        private IEnumerator Uploadform()
        {

            using (UnityWebRequest www = UnityWebRequest.Put(uri+"/"+DbId, "{}"))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    if(counter>1)
                    {
                        CancelInvoke("Upload");
                        
                    }
                    counter += 1;
                }
                else
                {
                    counter = 0;
                    Debug.Log("Upload complete!");
                }
            }
        }
        private void Upload()
        {
            StartCoroutine(Uploadform());
        }

        private void ClearData()
        {
            clientData.Clear();
            clientIdToGuid.Clear();
            clientSceneMap.Clear();

            gameInProgress = false;
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            if (connectionData.Length > MaxConnectionPayload)
            {
                callback(false, 0, false, null, null);
                return;
            }

            string payload = Encoding.UTF8.GetString(connectionData);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

            ConnectStatus gameReturnStatus = ConnectStatus.Success;

            // This stops us from running multiple standalone builds since 
            // they disconnect eachother when trying to join
            //
            // if (clientData.ContainsKey(connectionPayload.clientGUID))
            // {
            //     ulong oldClientId = clientData[connectionPayload.clientGUID].ClientId;
            //     StartCoroutine(WaitToDisconnectClient(oldClientId, ConnectStatus.LoggedInAgain));
            // }

            if (gameInProgress)
            {
                gameReturnStatus = ConnectStatus.GameInProgress;
            }
            else if (clientData.Count >= maxPlayers)
            {
                gameReturnStatus = ConnectStatus.ServerFull;
            }

            if (gameReturnStatus == ConnectStatus.Success)
            {
                clientSceneMap[clientId] = connectionPayload.clientScene;
                clientIdToGuid[clientId] = connectionPayload.clientGUID;
                clientData[connectionPayload.clientGUID] = new PlayerData(connectionPayload.playerName, clientId);
            }

            callback(false, 0, true, null, null);

            gameNetPortal.ServerToClientConnectResult(clientId, gameReturnStatus);

            if (gameReturnStatus != ConnectStatus.Success)
            {
                StartCoroutine(WaitToDisconnectClient(clientId, gameReturnStatus));
            }
        }

        private IEnumerator WaitToDisconnectClient(ulong clientId, ConnectStatus reason)
        {
            gameNetPortal.ServerToClientSetDisconnectReason(clientId, reason);

            yield return new WaitForSeconds(0);

            KickClient(clientId);
        }

        private void KickClient(ulong clientId)
        {
            NetworkObject networkObject = NetworkSpawnManager.GetPlayerNetworkObject(clientId);
            if (networkObject != null)
            {
                networkObject.Despawn(true);
            }

            NetworkManager.Singleton.DisconnectClient(clientId);
        }
    }
}