using System.Collections.Generic;
using Game_Systems.Utility;
using MLAPI;
using Network;
using UnityEngine;

namespace Game_Systems
{
    public class StartGameManager : MonoBehaviour
    {
        private void Start() {
            if(NetworkManager.Singleton.IsServer)
            {
                List<Vector3> pos = RespawnPointGenerator.generatePoints(10, 70);
                foreach (var playerManager in GameObject.FindGameObjectsWithTag("PlayerManager"))
                {
                    PlayerManager manager = playerManager.GetComponent<PlayerManager>();

                    int rand = RespawnPointGenerator.rnd.Next(pos.Count);

                    manager.SpawnCharacter(pos[rand]);

                    pos.RemoveAt(rand);
                }
            }
            
        }
    }
}
