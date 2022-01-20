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
        private int playerCount;
        private float mapRadius;
        private List<List<Vector3>> dividedPoints = new List<List<Vector3>>();

        private void Awake() {
            mapRadius = 62f;
        }

        public void UpdateGameInfo(GameInfo gameInfo)
        {
            gameMode = gameInfo.gameMode;
            teamCount = gameInfo.teamCount;
            playerCount = gameInfo.playerCount;
            GenerateStartingSpawnPoints(gameInfo);
        }

        public void GenerateStartingSpawnPoints(GameInfo gameInfo)
        {
            if(gameMode == GameMode.FreeForAll)
            {
                dividedPoints = RespawnPointGenerator.GenerateListsOfDividedPoints(playerCount, 1, mapRadius);
            }else if(gameMode == GameMode.TeamDeathmatch)
            {
                dividedPoints = RespawnPointGenerator.GenerateListsOfDividedPoints(teamCount, gameInfo.maxPlayersPerTeam, mapRadius);
            }
        }

        public void SpawnPlayer(PlayerManager manager)
        {
            GameObject player = null;
            if(gameMode == GameMode.FreeForAll)
            {
                int spawnPoint = RespawnPointGenerator.rnd.Next(dividedPoints.Count);
                player = manager.SpawnCharacter(dividedPoints[spawnPoint][0]);
                dividedPoints.RemoveAt(spawnPoint);
            }else if(gameMode == GameMode.TeamDeathmatch)
            {
                int teamId = manager.GetPlayerState().TeamId - 1;
                int spawnPoint = RespawnPointGenerator.rnd.Next(dividedPoints[teamId].Count);
                player = manager.SpawnCharacter(dividedPoints[teamId][spawnPoint]);
                dividedPoints[teamId].RemoveAt(spawnPoint);
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
                if(pm.GetPlayerState().TeamId == teamId)
                {
                    count++;
                }
            }
            return count;
        }
    }
}


