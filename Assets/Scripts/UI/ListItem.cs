using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItem : MonoBehaviour
{
    protected GameObject listitemObject;

    public ListItem(GameObject obj)
    {
        listitemObject = obj;
    }

    public void SetActive(bool active)
    {
        listitemObject.SetActive(active);
    }

    public void SetParent(GameObject parent)
    {
        listitemObject.transform.SetParent(parent.transform, false);
    }

    public void SetPosition(Vector3 pos)
    {
        listitemObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
        listitemObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void DestroyGameObject()
    {
        Destroy(listitemObject);
    }
}
