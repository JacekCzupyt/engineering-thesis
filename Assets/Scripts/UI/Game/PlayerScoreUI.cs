using System.Collections.Generic;
using Network;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    public class PlayerScoreUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject scoreListViewObject;
        [SerializeField] private GameObject gameModeTitle;
        [SerializeField] private GameObject gameModeScorePanelObject;
        [SerializeField] private GameObject TeamScoreTextPrefab;
        private List<GameObject> teamScoreTexts = new List<GameObject>();
        private ListView.ListView scoreListView;

        private void Awake()
        {
            scoreListView = scoreListViewObject.GetComponent<ListView.ListView>();
        }

        private void InitializeScoreListView()
        {
            scoreListView = scoreListViewObject.GetComponent<ListView.ListView>();
        }

        public void CreateListItem(PlayerState state, float position, bool IsPlayerId)
        {
            GameObject obj = Instantiate(scoreListView.prefab, new Vector3(0, position, 0), 
                Quaternion.identity) as GameObject;

            ScoreListItem item = new ScoreListItem(obj, state);
            if(IsPlayerId) item.HighlightPlayerName();
            scoreListView.AddItem(item);
            
        }

        public void DestroyCards()
        {
            scoreListView.ClearList();
        }

        public void UpdateGameMode(GameMode gamemode)
        {
            gameModeTitle.GetComponent<Text>().text = gamemode.ToString();
            gameModeScorePanelObject.transform.GetChild(0).GetComponent<Text>().text = GameModeAbbreviation(gamemode);
        }

        public string GameModeAbbreviation(GameMode mode)
        {
            if(mode == GameMode.FreeForAll)
            {
                return "FFA";
            }else if(mode == GameMode.TeamDeathmatch)
            {
                return "TDM";
            }
            return "";
        }

        public void AddTeamScores(int teamId, int teamScoreValue)
        {
            float pos = -(100 + 50*(teamId - 1));
            var teamScore = Instantiate(TeamScoreTextPrefab, 
            new Vector3(0, pos, 0), Quaternion.identity) as GameObject;

            teamScore.GetComponent<Text>().text = "Team " + teamId +": " + teamScoreValue;

            teamScore.transform.SetParent(gameModeScorePanelObject.transform, false);
            teamScore.SetActive(true);
            teamScoreTexts.Add(teamScore);
        }

        public void DeleteTeamScores()
        {
            foreach(var teamScore in teamScoreTexts)
            {
                Destroy(teamScore);
            }
            teamScoreTexts.Clear();
        }
    }
}
