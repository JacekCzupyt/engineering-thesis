using System.Collections;
using System.Collections.Generic;
using MLAPI;
using Network;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    void Start()
    {
        if (NetworkManager.Singleton.IsServer) {
            foreach(var playerManager in GameObject.FindGameObjectsWithTag("PlayerManager")) {
                PlayerManager manager = playerManager.GetComponent<PlayerManager>();
                manager.SpawnCharacter();
            }
        }
    }
}
