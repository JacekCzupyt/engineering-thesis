using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListView : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject prefab;

    private List<ListItem> listItems;

    private void Start()
    {
        listItems = new List<ListItem>();
    }

    public void AddItem(ListItem item)
    {
        listItems.Add(item);
    }

    public void ClearList()
    {
        foreach(ListItem item in listItems)
        {
            item.DestroyGameObject();
        }
        listItems.Clear();
    }


}
