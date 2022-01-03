using NetPortals;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AddServerManager : MonoBehaviour
{
    [SerializeField] private InputField serverName;
    [SerializeField] private InputField ipAddressTextInput;
    [SerializeField] private InputField playerName;
    [SerializeField] private GameObject nameInputError;
    [SerializeField] private GameObject ipInputError;
    [SerializeField] private GameObject playerInputError;
    [SerializeField] private GameObject pleaseWaitMessage;
    [SerializeField] private GameObject addingPanel;

    string uri = "http://79.191.62.119:8080/servers";
    public void SendData()
    {
        if (!NameValidation())
            return;
        string jsonData="{\"name\": \""+serverName.text+"\", \"ip\": \""+ipAddressTextInput.text+"\"}" ;
        StartCoroutine(Send_Data_Coroutine(uri,jsonData));
    }
    private IEnumerator Send_Data_Coroutine(string Uri, string data)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(uri,""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.SetRequestHeader("Content-Type", "application/json");
            addingPanel.SetActive(false);
            pleaseWaitMessage.SetActive(true);
            yield return www.SendWebRequest();
            pleaseWaitMessage.SetActive(false);

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                PlayerPrefs.SetString("PlayerName", playerName.text);
                GameNetPortal.Instance.StartHost();
            }

        }
    }
    private bool NameValidation()
    {
        IPAddress Ip;
        if (serverName.text.Length <= 0)
        {
            nameInputError.SetActive(true);
            return false;
        }
        string[] k = ipAddressTextInput.text.Split('.');        
        if (ipAddressTextInput.text.Length<=0 || k.Length!=4 ||!IPAddress.TryParse(ipAddressTextInput.text,out Ip))
        {
            ipInputError.SetActive(true);
            return false;
        }
        if(playerName.text.Length<=0)
        {
            playerInputError.SetActive(true);
            return false;
        }
        return true;
    }
}
