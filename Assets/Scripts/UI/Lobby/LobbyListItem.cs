using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class LobbyListItem : ListItem
{
    public LobbyListItem(GameObject playerCard, LobbyPlayerState state) : base(playerCard)
    {
        ListitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
        ListitemObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = GetReadyIndicatorColor(state.IsReady);
    }

    private Color GetReadyIndicatorColor(bool val)
    {
        if(val) return Color.green;
        else return Color.red;
    }

    public void UpdateCard(LobbyPlayerState state)
    {
        ListitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
        ListitemObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = GetReadyIndicatorColor(state.IsReady);
    }
}
