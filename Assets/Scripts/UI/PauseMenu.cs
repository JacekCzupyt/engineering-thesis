using Input_Systems;
using MLAPI;
using NetPortals;
using UnityEngine;

namespace UI {
    public class PauseMenu : NetworkBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        [SerializeField] GameObject settingsMenu;
        private CharacterInputManager characterInput;


    
        void Start() {
            characterInput = CharacterInputManager.Instance;
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(pauseMenu.activeSelf || settingsMenu.activeSelf)
                {
                    Resume();
                }else{
                    Pause();
                }
            }
        }
        void Pause()
        {
            pauseMenu.SetActive(true);
            characterInput.enabled = false;
            Cursor.lockState = CursorLockMode.None;

        }
        public void GoBack()
        {
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);

        }
        public void Resume()
        {
            //Debug.Log("is work");
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            characterInput.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Settings()
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }

        public void LeaveGame()
        {
            if (IsOwner)
            {
                characterInput.enabled = true;
                GameNetPortal.Instance.RequestDisconnect();
            }
        }
        public void QuitGame()
        {
            if(IsOwner)
            {
                GameNetPortal.Instance.RequestDisconnect();
                Application.Quit();
            }
        }
    }
}
