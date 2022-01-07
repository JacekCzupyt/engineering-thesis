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

        private void Awake() {
            if(NetworkManager.Singleton.IsServer)
            {
                gameManager.SpawnPlayerEvent += SpawnPlayer;
            }
        }
        private void Start() {
            var gameInfoObject = GameObject.FindGameObjectWithTag("GameInfo");
            GameInfo gameInfo = gameInfoObject.GetComponent<GameInfo>();
            gameMode = gameInfo.GameMode;
            teamCount = gameInfo.TeamCount;

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

        private void OnDestroy() {
            if(NetworkManager.Singleton)
            {
                gameManager.SpawnPlayerEvent -= SpawnPlayer;
            }
        }

        private void OnGameStart()
        {
            

        }

        private void SpawnPlayer(object sender, PlayerManager manager)
        {
            if(gameMode == GameMode.FreeForAll)
            {
                manager.SpawnCharacter(new Vector3(0,0,0));
                Debug.Log("Spawn Player");
            }
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


