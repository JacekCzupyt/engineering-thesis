using System.Collections.Generic;
using MLAPI;
using Network;
using UnityEngine;
using Game_Systems.Utility;
public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject playerScoreUIObject;
    private PlayerScoreUI playerScoreUI;
    List<PlayerManager> playerManagers = new List<PlayerManager>();
    void Start()
    {
        playerScoreUI = playerScoreUIObject.GetComponent<PlayerScoreUI>();
        if (NetworkManager.Singleton.IsServer) {
            List<Vector3> pos = RespawnPointGenerator.generatePoints(GameObject.FindGameObjectsWithTag("PlayerManager").Length);
            foreach (var playerManager in GameObject.FindGameObjectsWithTag("PlayerManager")) {
                PlayerManager manager = playerManager.GetComponent<PlayerManager>();
                playerManagers.Add(manager);
                int rand=RespawnPointGenerator.rnd.Next(pos.Count);
                manager.SpawnCharacter(pos[rand]);
                pos.RemoveAt(rand);
            }
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            playerScoreUIObject.SetActive(true);
            playerScoreUI.DestroyCards();
            for(int i = 0; i < playerManagers.Count; i++)
            {
                float position = -(30*i + 30*(i+1));
                playerScoreUI.CreateListItem(playerManagers[i].ToPlayerScoreState(), position);
            }
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            playerScoreUIObject.SetActive(false);
        }
    }
}
