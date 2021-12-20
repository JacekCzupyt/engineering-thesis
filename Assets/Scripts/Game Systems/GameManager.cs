using System.Collections.Generic;
using Game_Systems.Utility;
using MLAPI;
using Network;
using UnityEngine;

namespace Game_Systems {
    public class GameManager : MonoBehaviour {

        [SerializeField] private GameObject scoreboardManagerObject;
        [SerializeField] private GameObject gamestateManagerObject;
        private void Start()
        {
            if (NetworkManager.Singleton.IsServer) {
                List<Vector3> pos = RespawnPointGenerator.generatePoints(10, 70);
                foreach (var playerManager in GameObject.FindGameObjectsWithTag("PlayerManager")) {
                    PlayerManager manager = playerManager.GetComponent<PlayerManager>();
                    manager.SetScoreBoardManager(scoreboardManagerObject.GetComponent<ScoreboardManager>());
                    manager.SetGameStateManager(gamestateManagerObject.GetComponent<GameStateManager>());
                    int rand=RespawnPointGenerator.rnd.Next(pos.Count);
                    manager.SpawnCharacter(pos[rand]);
                    pos.RemoveAt(rand);
                }
                Debug.Log("Game Manager End");
            }
        }
    }
}
