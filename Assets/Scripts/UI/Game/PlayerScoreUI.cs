using Network;
using UnityEngine;

namespace UI.Game {
    public class PlayerScoreUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject scoreListViewObject;
        [SerializeField] private GameObject scoreboardManager;
        private ListView.ListView scoreListView;

        private void Awake()
        {
            scoreListView = scoreListViewObject.GetComponent<ListView.ListView>();
        }

        private void InitializeScoreListView()
        {
            scoreListView = scoreListViewObject.GetComponent<ListView.ListView>();
        }

        public void CreateListItem(PlayerState state, float position)
        {
            GameObject obj = Instantiate(scoreListView.prefab, new Vector3(0, position, 0), 
                Quaternion.identity) as GameObject;

            ScoreListItem item = new ScoreListItem(obj, state);
            scoreListView.AddItem(item);
        }

        public void DestroyCards()
        {
            scoreListView.ClearList();
        }
    }
}
