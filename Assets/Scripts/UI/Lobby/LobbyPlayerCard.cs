using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby {
    public class LobbyPlayerCard
    {
        private GameObject lobbyPlayerCard;
        private GameObject cardText;

        public LobbyPlayerCard(string playerName, bool isReady, bool isHost, GameObject parent)
        {
            lobbyPlayerCard = CreateTextObject(playerName);
            lobbyPlayerCard.transform.SetParent(parent.transform);
        }

        public GameObject CreateTextObject(string objectText)
        {
            GameObject obj = new GameObject("playerNameText");
            obj.AddComponent<RectTransform>();
            Text t = obj.AddComponent(typeof(Text)) as Text;
            t.text = objectText;
            t.fontSize = 32;
            t.color = Color.white;
            t.alignment = TextAnchor.MiddleLeft;
            return obj;
        }
    }
}
