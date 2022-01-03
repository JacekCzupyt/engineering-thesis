using NetPortals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        string jsonData = "{\"name\": \"" + serverName.text + "\", \"ip\": \"" + ipAddressTextInput.text + "\"}";
        StartCoroutine(Test_Connection(uri, jsonData));
    }
    private IEnumerator Test_Connection(string uri, string data)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(uri, ""))
        {

            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.SetRequestHeader("Content-Type", "application/json");
            addingPanel.SetActive(false);
            pleaseWaitMessage.SetActive(true);
            Thread thread = new Thread(() => { StartTcpServerForConnectionTest(); });
            thread.Start();
            yield return www.SendWebRequest();
            bool finished=thread.Join(1);
            if (!finished)
            {
                thread.Abort();
            }
            if (www.result != UnityWebRequest.Result.Success)
            {
                pleaseWaitMessage.SetActive(false);
                addingPanel.SetActive(true);
                Debug.Log(www.error);
            }
            else
            {
                PlayerPrefs.SetString("PlayerName", playerName.text);
                GameNetPortal.Instance.StartHost();
            }
        }
    }

    private void StartTcpServerForConnectionTest()
    {
            Debug.Log("Waiting for connection");
            TcpListener listener = new TcpListener(IPAddress.Any, 7777);
            listener.Server.ReceiveTimeout=5000;
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            Debug.Log("Client accepted");
            client.Close();
            listener.Stop();            
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
