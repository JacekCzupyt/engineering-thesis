using System.Collections.Generic;
using Game_Systems.Utility;
using MLAPI;
using Network;
using UnityEngine;

namespace Game_Systems {
    public class GameManager : MonoBehaviour {

        [SerializeField] private GameObject scoreboardManagerObject;
        private void Start()
        {
            if (NetworkManager.Singleton.IsServer) {
                List<Vector3> pos = RespawnPointGenerator.generatePoints(GameObject.FindGameObjectsWithTag("PlayerManager").Length);
                foreach (var playerManager in GameObject.FindGameObjectsWithTag("PlayerManager")) {
                    PlayerManager manager = playerManager.GetComponent<PlayerManager>();
                    manager.SetScoreBoardManager(scoreboardManagerObject.GetComponent<ScoreboardManager>());
                    int rand=RespawnPointGenerator.rnd.Next(pos.Count);
                    manager.SpawnCharacter(pos[rand]);
                    pos.RemoveAt(rand);
                }
                Debug.Log("Game Manager End");
            }
        }
    }
}
