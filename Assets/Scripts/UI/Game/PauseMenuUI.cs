using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input_Systems;
using NetPortals;
using UnityEngine.UI;

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
            TogglePlayerInput(false);
            IsPaused = false;
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(IsPaused)
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
            TogglePlayerInput(false);
            IsPaused = false;
        }

        private void PauseGame()
        {
            pauseMenuObject.SetActive(true);
            TogglePlayerInput(true);
            IsPaused = true;
        }

        private void TogglePlayerInput(bool pause)
        {
            if(pause)
            {
                characterInputManager.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }else
            {  
                characterInputManager.enabled = true;
                Cursor.lockState =CursorLockMode.Locked;
            }

        }

        public void LeaveGame()
        {
            TogglePlayerInput(true);
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


