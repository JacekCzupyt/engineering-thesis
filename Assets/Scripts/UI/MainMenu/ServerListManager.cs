using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using NetPortals;

namespace UI.MainMenu
{
    [Serializable]
    public class ServerComponent
    {
        public int id;
        public string ip;
        public string name;
        
        public ServerComponent(int _id,string _ip,string _name)
        {
            id = _id;
            ip = _ip;
            name = _name;
        }
    }
    public class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
    public class ServerListManager : MonoBehaviour
    {
        [SerializeField] private GameObject ListItemPrefab;
        [SerializeField] private Transform ItemHolder;
        [SerializeField] private GameObject loadingInfo;
        private ServerComponent[] ServerArray;
        [SerializeField] private GameObject isacticeServer;
        
        // Start is called before the first frame update
        string uri = "http://79.191.52.229:8080/servers";

        public void getData()
        {
            foreach(Transform child in ItemHolder)
            {
                Destroy(child.gameObject);
            }    
            StartCoroutine(Get_Data_Coroutine(uri));

        }
        private IEnumerator Get_Data_Coroutine(string Uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                loadingInfo.SetActive(true);
                yield return webRequest.SendWebRequest();
                loadingInfo.SetActive(false);
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        ServerArray = JsonHelper.getJsonArray<ServerComponent>(webRequest.downloadHandler.text);
                        if(ServerArray.Length==0)
                        {
                            isacticeServer.SetActive(true);
                            break;
                        }
                        isacticeServer.SetActive(false);
                        for (int i = 0; i < ServerArray.Length; i++)
                        {
                            GameObject g;
                            g = Instantiate(ListItemPrefab, ItemHolder.transform);
                            ItemClassHolder o = (ItemClassHolder) g.GetComponent(typeof(ItemClassHolder));
                            o.serv = new ServerComponent(ServerArray[i].id, ServerArray[i].ip, ServerArray[i].name);                         
                            g.GetComponentInChildren<Text>().text = o.serv.name;
                            g.GetComponentInChildren<Button>().onClick.AddListener(delegate { JoinClick(o.serv.ip); });
                        }
                        break;
                }
            }
        }
        private void JoinClick(string ip)
        {
            ClientGameNetPortal.Instance.SetConnectAddress(ip);
            ClientGameNetPortal.Instance.StartClient();
        }
    }
}

