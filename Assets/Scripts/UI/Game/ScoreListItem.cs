using Network;
using UI.ListView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game {
    public class ScoreListItem : ListItem
    {

        public ScoreListItem(GameObject playerCard, PlayerState state) : base(playerCard)
        {
            UpdateCard(state);
        }

        public void UpdateCard(PlayerState state)
        {
            ListitemObject.GetComponent<Image>().color = TeamColor.GetTeamColor(state.TeamId, 0.4f);
            ListitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
            ListitemObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = state.PlayerKills.ToString();
            ListitemObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = state.PlayerDeaths.ToString();
        }

        public void HighlightPlayerName()
        {
            ListitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = new Color(255, 150, 0, 255);
            ListitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
        }
    }
}
