using System.Collections.Generic;
using UnityEngine;

namespace UI.ListView {
    public class ListView : MonoBehaviour
    {
        [SerializeField] public GameObject panel;
        [SerializeField] public GameObject prefab;

        private List<ListItem> listItems = new List<ListItem>();

        public void AddItem(ListItem item)
        {
            item.SetParent(panel);
            item.SetActive(true);
            listItems.Add(item);
        }

        public void ClearList()
        {
            foreach(ListItem item in listItems)
            {
                Destroy(item.ListitemObject);
            }
            listItems.Clear();
        }


    }
}
