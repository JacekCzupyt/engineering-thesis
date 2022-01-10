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
            ListitemObject.GetComponent<Image>().color = GetTeamColor(state.TeamId);
            ListitemObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = state.PlayerName;
            ListitemObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = state.PlayerKills.ToString();
            ListitemObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = state.PlayerDeaths.ToString();
        }

        public Color GetTeamColor(int teamNumber){
            Color c = Color.grey;
            switch(teamNumber){
                case 1: c = Color.red;
                break;
                case 2: c = Color.blue;
                break;
                case 3: c = Color.green;
                break;
                case 4: c = Color.yellow;
                break;
                default: break;
            }
            c.a = 0.4f;
            return c;
        }

    }
}
