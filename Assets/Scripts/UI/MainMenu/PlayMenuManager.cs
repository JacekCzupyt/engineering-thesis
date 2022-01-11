using NetPortals;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu {
    public class PlayMenuManager : MonoBehaviour
    {
        [Header("Play Menu Panel References")]
        [SerializeField] private GameObject joinServerMenu;
        [SerializeField] private GameObject createServerMenu;
        [SerializeField] private GameObject joinGameMenu;
        [SerializeField] private GameObject playMenu;
        [Header("Other UI References")]
        [SerializeField] private InputField playerNameInput;
        [SerializeField] private InputField ipAddressTextInput;
        [SerializeField] private GameObject nameErrorText;
        [Header("Server List References")]
        [SerializeField] private ServerListManager serv;

        public void HostPrivateGame()
        {
            //Currently no additional host menus
            if(!NameValidation()) return;
            GameNetPortal.Instance.StartHost();
        }

        public void JoinPrivateGame()
        {
            if(!NameValidation()) return;
            SwitchMenus(playMenu, joinGameMenu);
        }

        public void JoinPrivateGameConnect()
        {
            //Ip address validation
            ClientGameNetPortal.Instance.SetConnectAddress(ipAddressTextInput.text);
            ClientGameNetPortal.Instance.StartClient();
        }

        public void CreateServer()
        {
            if(!NameValidation()) return;
            SwitchMenus(playMenu, createServerMenu);
        }

        public void JoinServer()
        {
            if(!NameValidation()) return;
            SwitchMenus(playMenu, joinServerMenu);
            serv.getData();
        }

        public void BackToPlayMenu(int index)
        {
            switch(index)
            {
                case 0: SwitchMenus(joinServerMenu, playMenu);
                break;
                case 1: SwitchMenus(createServerMenu, playMenu);
                break;
                case 2: SwitchMenus(joinGameMenu, playMenu);
                break;
                default: break;
            }
        }

        private void SwitchMenus(GameObject disable, GameObject enable)
        {
            disable.SetActive(false);
            enable.SetActive(true);
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
    }
}
