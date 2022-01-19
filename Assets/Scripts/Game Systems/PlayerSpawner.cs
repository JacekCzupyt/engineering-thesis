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
        private float mapRadius;
        private List<List<Vector3>> dividedPoints = new List<List<Vector3>>();

        private void Start() {
            mapRadius = 70f;
        }

        public void ReceiveGameInfo(GameInfo gameInfo)
        {
            gameMode = gameInfo.gameMode;
            teamCount = gameInfo.teamCount;
            
            if(gameMode == GameMode.FreeForAll)
            {
                dividedPoints = RespawnPointGenerator.GenerateListsOfDividedPoints(gameInfo.playerCount, 1, mapRadius);
            }else if(gameMode == GameMode.TeamDeathmatch)
            {
                dividedPoints = RespawnPointGenerator.GenerateListsOfDividedPoints(teamCount, gameInfo.maxPlayersPerTeam, mapRadius);
            }
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
                if(pm.GetPlayerState().TeamId == teamId)
                {
                    count++;
                }
            }
            return count;
        }
    }
}


