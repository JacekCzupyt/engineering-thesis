using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NotifyServerBrowser : NetworkBehaviour
{
    private string uri = "http://79.191.62.119:8080/servers";
    private string IpAddr;
    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            //StartCoroutine(GetIPAddress());
            //InvokeRepeating("NotifyServer", 0.0f, 10.0f);
        }
    }

    
    private void NotifyServer()
    {
        Debug.Log("Hi+ " + IpAddr);
        //StartCoroutine(Upload());
    }
    private IEnumerator Upload()
    {

        using (UnityWebRequest www = UnityWebRequest.Put(uri, ""))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
        }
    }
}
