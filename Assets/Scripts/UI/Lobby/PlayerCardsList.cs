using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby {
    public class PlayerCardsList : MonoBehaviour
    {
        [SerializeField] private GameObject listItemPrefab;
        [SerializeField] private GameObject listContainer;

        public void CreatePlayerCard(string text, bool isready, bool ishost)
        {   
            GameObject obj = Instantiate(listItemPrefab, new Vector3(0, 30, 0), Quaternion.identity) as GameObject;
            obj.GetComponent<Text>().text = text;
            obj.GetComponent<Image>().color = Color.magenta;
            obj.transform.SetParent(listContainer.transform);
            obj.SetActive(true);
        }
    }
}
