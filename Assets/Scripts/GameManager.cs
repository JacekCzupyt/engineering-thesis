using MLAPI;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] GameObject can;
    void Start()
    {
        can.SetActive(false);
    }
    public void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            StartButtons(can);
        }
        else {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    private static void StartButtons(GameObject cross) {
        if (GUILayout.Button("Host")) { NetworkManager.Singleton.StartHost(); cross.SetActive(true); }
        if (GUILayout.Button("Client")) { NetworkManager.Singleton.StartClient(); cross.SetActive(true); }
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        
    }
    
    private static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
}
