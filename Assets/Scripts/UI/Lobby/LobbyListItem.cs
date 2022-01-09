using Network;
using UI.ListView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby {
    public class LobbyListItem : ListItem
    {
        public LobbyListItem(GameObject playerCard, LobbyPlayerState state) : base(playerCard)
        {
            UpdateCard(state);
        }

        private Color GetReadyIndicatorColor(bool val)
        {
            if(val) return Color.green;
            else return Color.red;
        }

        public void UpdateCard(LobbyPlayerState state)
        {
            ListitemObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = GetTeamColor(state.TeamId);
            ListitemObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = state.PlayerName;
            ListitemObject.transform.GetChild(2).gameObject.GetComponent<Image>().color = GetReadyIndicatorColor(state.IsReady);
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
