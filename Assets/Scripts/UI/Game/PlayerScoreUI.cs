using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject scoreListViewObject;
    [SerializeField] private GameObject gameManager;
    private ListView scoreListView;

    private void Awake()
    {
        scoreListView = scoreListViewObject.GetComponent<ListView>();
    }

    public void CreateListItem(ScorePlayerState state, float position)
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
