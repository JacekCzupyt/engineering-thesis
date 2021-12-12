using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListView : MonoBehaviour
{
    [SerializeField] public GameObject panel;
    [SerializeField] public GameObject prefab;

    private List<ListItem> listItems;

    private void Awake()
    {
        listItems = new List<ListItem>();
    }

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
