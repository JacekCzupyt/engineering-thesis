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

namespace UI.MainMenu
{
    public class AddServerManager : MonoBehaviour
    {
        [SerializeField] public InputField serverNameInput;
        [SerializeField] private GameObject serverNameInputError;
        [SerializeField] private GameObject pleaseWaitMessage;
        [SerializeField] private GameObject addingPanel;

        //string uri = "http://79.191.52.229:8080/servers";
        public string ip;
        // public bool IsAdd=false;
        private void Start()
        {
            StartCoroutine(GetIPAddress());
        }
        private IEnumerator GetIPAddress()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://checkip.dyndns.org");
            pleaseWaitMessage.SetActive(true);
            yield return www.SendWebRequest();
            pleaseWaitMessage.SetActive(false);
            switch (www.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + www.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + www.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string result = www.downloadHandler.text;
                    string[] a = result.Split(':'); // Split into two substrings -> one before : and one after. 
                    string a2 = a[1].Substring(1);  // Get the substring after the :
                    string[] a3 = a2.Split('<');    // Now split to the first HTML tag after the IP address.
                    ip = a3[0];              // Get the substring before the tag.
                    Debug.Log("External IP Address = " + ip);
                    ServerGameNetPortal.Instance.serverIp = ip;
                    break;
            }
        }
        public void SendData() {
            ServerGameNetPortal.Instance.informServerBrowser = true;
            // IsAdd = true;
            if (!ServerNameValidation())
                return;
            GameNetPortal.Instance.StartHost();
        }

        private bool ServerNameValidation() {
            var serverName = serverNameInput.text;
            if (serverName.Length <= 0)
            {
                serverNameInputError.SetActive(true);
                return false;
            }
            serverNameInputError.SetActive(false);
            PlayerPrefs.SetString("ServerName", serverName);
            return true;
        }
    }
}