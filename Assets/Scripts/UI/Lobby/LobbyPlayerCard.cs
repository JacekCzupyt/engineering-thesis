using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class LobbyPlayerCard
{
    public GameObject playerCard;
    public LobbyPlayerCard(GameObject pcard, LobbyPlayerState state)
    {
        playerCard = pcard;
        playerCard.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
        playerCard.transform.GetChild(1).gameObject.GetComponent<Image>().color = GetReadyIndicatorColor(state.IsReady);
    }

    private Color GetReadyIndicatorColor(bool val)
    {
        if(val) return Color.green;
        else return Color.red;
    }

    public void SetActive(bool active)
    {
        playerCard.SetActive(active);
    }

    public void SetParent(GameObject obj)
    {
        playerCard.transform.SetParent(obj.transform, false);
    }

    public void SetPostion(float pos)
    {
        playerCard.transform.position = new Vector3(0, pos, 0);
        playerCard.transform.localScale = new Vector3(1, 1, 1);
    }

    public void UpdateCard(LobbyPlayerState state)
    {
        playerCard.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
        playerCard.transform.GetChild(1).gameObject.GetComponent<Image>().color = GetReadyIndicatorColor(state.IsReady);
    }
}
