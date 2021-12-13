using UnityEngine;
using UnityEngine.UI;

public class ScoreListItem : ListItem
{

    public ScoreListItem(GameObject playerCard, ScorePlayerState state) : base(playerCard)
    {
        UpdateCard(state);
    }

    public void UpdateCard(ScorePlayerState state)
    {
        ListitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
        ListitemObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = state.PlayerKills.ToString();
        ListitemObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = state.PlayerDeaths.ToString();
    }

}