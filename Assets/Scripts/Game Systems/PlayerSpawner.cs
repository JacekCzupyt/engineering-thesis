using System;
using System.Collections.Generic;
using UnityEngine;
using Network;
using Game_Systems.Utility;
using MLAPI;

namespace Game_Systems
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] GameManager gameManager;
        private GameMode gameMode;
        private int teamCount;

        private void Start() {
            if(NetworkManager.Singleton.IsServer)
            {
                // if(gameManager)
                // {
                //     gameManager.SpawnPlayerEvent += SpawnPlayer;
                // }
                // var players = GameObject.FindGameObjectsWithTag("PlayerManager");
                // List<Vector3> spawnPoints = null;
                // switch(gameMode)
                // {
                //     case GameMode.FreeForAll:
                //     spawnPoints = RespawnPointGenerator.generatePoints(10, 70);
                //     foreach (var player in players)
                //     {
                //         PlayerManager playerManager = player.GetComponent<PlayerManager>();

                //         int rand = RespawnPointGenerator.rnd.Next(spawnPoints.Count);

                //         playerManager.SpawnCharacter(spawnPoints[rand]);

                //         spawnPoints.RemoveAt(rand);
                //     }
                //     break;
                //     case GameMode.TeamDeathmatch:
                //     for(int i = 1; i < teamCount+1; i++)
                //     {
                //         int playerCount = GetPlayerCountPerTeam(players, i);
                //         spawnPoints = RespawnPointGenerator.GenerateTeamPoints(playerCount, teamCount, i, 70);
                //         foreach(var player in players)
                //         {
                //             PlayerManager playerManager = player.GetComponent<PlayerManager>();
                //             if(playerManager.teamId == i)
                //             {
                //                 int rand = RespawnPointGenerator.rnd.Next(spawnPoints.Count);

                //                 playerManager.SpawnCharacter(spawnPoints[rand]);

                //                 spawnPoints.RemoveAt(rand);
                //             }
                //         }
                //     }
                //     break;
                //     default: 
                //         throw new InvalidOperationException("Game Mode does not exist");
                // }
            }
            // Debug.Log("Team " + 1);
            // foreach(var point in RespawnPointGenerator.GenerateTeamPoints(5, 2, 1))
            // {
            //     Debug.Log("x: " + point.x + ", y: " + point.y + ", z: " + point.z);
            // }
            // Debug.Log("Team " + 2);
            // foreach(var point in RespawnPointGenerator.GenerateTeamPoints(5, 2, 2))
            // {
            //     Debug.Log("x: " + point.x + ", y: " + point.y + ", z: " + point.z);
            // }
        }

        private void OnGameStart()
        {
            

        }

        public void ReceiveGameInfo(GameInfo gameInfo)
        {
            gameMode = gameInfo.gameMode;
            teamCount = gameInfo.teamCount;
        }

        public void SpawnPlayer(PlayerManager manager)
        {
            GameObject player = null;
            List<Vector3> pos = RespawnPointGenerator.generatePoints(10, 70);
            if(gameMode == GameMode.FreeForAll)
            {
                int rand = RespawnPointGenerator.rnd.Next(pos.Count);
                player = manager.SpawnCharacter(pos[rand]);
                pos.RemoveAt(rand);

            }else if(gameMode == GameMode.TeamDeathmatch)
            {
                int rand = RespawnPointGenerator.rnd.Next(pos.Count);
                player = manager.SpawnCharacter(pos[rand]);
                pos.RemoveAt(rand);
            }
            player?.GetComponent<PlayerGameManager>().SetGameManager(gameManager);  
            player?.GetComponent<PlayerGameManager>().SetPlayerState(manager.GetPlayerState());              
        }

        private int GetPlayerCountPerTeam(GameObject[] players, int teamId)
        {
            int count = 0;
            foreach(var player in players)
            {
                PlayerManager pm = player.GetComponent<PlayerManager>();
                if(pm.teamId == teamId)
                {
                    count++;
                }
            }
            return count;
        }
    }
}


