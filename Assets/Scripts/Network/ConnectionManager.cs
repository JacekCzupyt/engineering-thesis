using System;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Connection;
using MLAPI.NetworkVariable.Collections;

namespace Network
{
    public class ConnectionManager : NetworkBehaviour
    {
        public static ConnectionManager Instance => instance;
        private static ConnectionManager instance;
        public event EventHandler<ulong> ClientConnectedEvent;
        public event EventHandler<ulong> ClientDisconnectEvent;

        private void Awake() {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public override void NetworkStart()
        {
            if(IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

                foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
                {
                    HandleClientConnected(client.ClientId);
                }
            }
        }

        private void OnDestroy() {
            if(NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            ClientConnectedEvent?.Invoke(this, clientId);
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            ClientDisconnectEvent?.Invoke(this, clientId);
        }
    } 
}

