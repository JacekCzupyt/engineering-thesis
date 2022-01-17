using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

namespace UI.Hud
{
    public class GameScoreUI : MonoBehaviour
    {
        [Header("Sprite Image References")]
        [SerializeField] private Sprite freeForAllSprite;
        [SerializeField] private Sprite teamDeathMatchSprite;

        [Header("UI References")]
        [SerializeField] private GameObject gameModeScoreImagePanelObject;
        [SerializeField] private GameObject gameScorePanelObject;
        [SerializeField] private GameObject TextPrefab;
        private List<GameObject> scoreTexts = new List<GameObject>();


        public void UpdateGameMode(GameMode gamemode)
        {
            gameModeScoreImagePanelObject.transform.GetChild(0).GetComponent<Image>().sprite = GameModeImageSprite(gamemode);
        }

        public Sprite GameModeImageSprite(GameMode mode)
        {
            if(mode == GameMode.FreeForAll)
            {
                return freeForAllSprite;
            }else if(mode == GameMode.TeamDeathmatch)
            {
                return teamDeathMatchSprite;
            }
            return null;
        }

        
        public void AddTeamScores(int teamId, int teamScoreValue)
        {
            int column = (int)Math.Floor((double)(teamId-1)/2);
            int row = (teamId-1)%2;
            float posX = 125 + column*250;
            float posY = -(50 + row*50);
            var teamScore = Instantiate(TextPrefab, 
            new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;

            teamScore.GetComponent<Text>().text = "Team " + teamId +": " + teamScoreValue;

            teamScore.transform.SetParent(gameScorePanelObject.transform, false);
            teamScore.SetActive(true);
            scoreTexts.Add(teamScore);
        }

        public void AddPlayerScores(string playerName, int killScore, int pos)
        {
            float posY = -(pos*50);
            var playerScore = Instantiate(TextPrefab,
            new Vector3(0, posY, 0), Quaternion.identity) as GameObject;

            playerScore.GetComponent<Text>().text = playerName + " " + killScore;

            playerScore.transform.SetParent(gameScorePanelObject.transform, false);
            playerScore.SetActive(true);
            scoreTexts.Add(playerScore);
        }

        public void DeleteScores()
        {
            foreach(var teamScore in scoreTexts)
            {
                Destroy(teamScore);
            }
            scoreTexts.Clear();
        }
    }
}
