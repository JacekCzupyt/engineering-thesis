using Network;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    public class PlayerScoreUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject scoreListViewObject;
        [SerializeField] private GameObject gameModeTitle;
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
        }
    }
}
