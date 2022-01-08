using NetPortals;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu {
    public class PlayMenuManager : MonoBehaviour
    {
        [SerializeField] private InputField playerNameInput;
        [SerializeField] private InputField ipAddressTextInput;
        [SerializeField] private GameObject nameErrorText;
        [SerializeField] private GameObject serverList;
        [SerializeField] private GameObject playMenu;
        [SerializeField] private ServerListManager serv;
        [SerializeField] private GameObject addServer;
        public void AddServer()
        {
            playMenu.SetActive(false);
            addServer.SetActive(true);
        }
        public void BackServerList()
        {
            playMenu.SetActive(true);
            serverList.SetActive(false);
        }
        public void BackAddServer()
        {
            playMenu.SetActive(true);
            addServer.SetActive(false);
        }
        public void ServerList()
        {
            serverList.SetActive(true);
            playMenu.SetActive(false);
            serv.getData();
        }
        public void HostGame(){
            if(NameValidation()) GameNetPortal.Instance.StartHost();
        }

        public void ClientConnect(){
            if(NameValidation()) 
            {
                ClientGameNetPortal.Instance.SetConnectAddress(ipAddressTextInput.text);
                ClientGameNetPortal.Instance.StartClient();
            }
        }

        private bool PlayMenuValidation()
        {
            if(NameValidation() && AddressValidation()) return true;
        
            return false;
        }

        private bool NameValidation(){
            string playerName = playerNameInput.text;
            if(playerName.Length <= 0)
            {
                nameErrorText.SetActive(true);
                return false;
            }
            nameErrorText.SetActive(false);
            PlayerPrefs.SetString("PlayerName", playerName);
            return true;
        }

        private bool AddressValidation()
        {
            string ipAddress = ipAddressTextInput.text;
            if(ipAddress.Length <= 0) return false;

            return true;
        }
    }
}
