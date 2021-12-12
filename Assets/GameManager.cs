using System.Collections;
using System.Collections.Generic;
using MLAPI;
using Network;
using UnityEngine;
using UnityEngine.InputSystem;
using Game_Systems.Utility;
public class GameManager : MonoBehaviour {
    void Start()
    {
        if (NetworkManager.Singleton.IsServer) {
            List<Vector3> pos = RespawnPointGenerator.generatePoints(GameObject.FindGameObjectsWithTag("PlayerManager").Length);
            foreach (var playerManager in GameObject.FindGameObjectsWithTag("PlayerManager")) {
                PlayerManager manager = playerManager.GetComponent<PlayerManager>();
                int rand=RespawnPointGenerator.rnd.Next(pos.Count);
                manager.SpawnCharacter(pos[rand]);
                pos.RemoveAt(rand);
            }
        }
    }
}
