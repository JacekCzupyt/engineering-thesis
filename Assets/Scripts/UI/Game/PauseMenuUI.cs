using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input_Systems;
using NetPortals;

namespace UI.Game
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuObject;
        [SerializeField] private GameObject settingsMenuObject;
        private CharacterInputManager characterInputManager;
        private bool IsPaused;
        private void Awake() {
            characterInputManager = CharacterInputManager.Instance;
            IsPaused = false;
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(pauseMenuObject.activeSelf || settingsMenuObject.activeSelf)
                {
                    ResumeGame();
                }else{
                    PauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            pauseMenuObject.SetActive(false);
            settingsMenuObject.SetActive(false);
            TogglePlayerInput();
        }

        private void PauseGame()
        {
            pauseMenuObject.SetActive(true);
            TogglePlayerInput();
        }

        private void TogglePlayerInput()
        {
            if(IsPaused)
            {  
                characterInputManager.enabled = true;
                Cursor.lockState =CursorLockMode.Locked;
                IsPaused = false;
            }else
            {
                characterInputManager.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                IsPaused = true;
            }
        }

        public void LeaveGame()
        {
            TogglePlayerInput();
            Cursor.lockState = CursorLockMode.None;
            GameNetPortal.Instance.RequestDisconnect();
        }

        public void QuitGame()
        {
            GameNetPortal.Instance.RequestDisconnect();
            Application.Quit();
        }

        public void Settings()
        {
            pauseMenuObject.SetActive(false);
            settingsMenuObject.SetActive(true);
        }

        public void ReturnToPauseMenu()
        {
            pauseMenuObject.SetActive(true);
            settingsMenuObject.SetActive(false); 
        }


    }
}


