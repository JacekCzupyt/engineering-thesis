using NetPortals;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu {
    public class PlayMenuManager : MonoBehaviour
    {
        [SerializeField] private InputField playerNameInput;
        [SerializeField] private InputField ipAddressTextInput;
        [SerializeField] private GameObject nameErrorText;

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
