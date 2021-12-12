using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class LobbyPlayerCard : ListItem
{
        public LobbyPlayerCard(GameObject playerCard, LobbyPlayerState state) : base(playerCard)
    {
        listitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
        listitemObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = GetReadyIndicatorColor(state.IsReady);
    }

    private Color GetReadyIndicatorColor(bool val)
    {
        if(val) return Color.green;
        else return Color.red;
    }

    public void UpdateCard(LobbyPlayerState state)
    {
        listitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
        listitemObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = GetReadyIndicatorColor(state.IsReady);
    }
}
