using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input_Systems;
using MLAPI;
public class PauseMenu : NetworkBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Behaviour script;
    private CharacterInputManager characterInput;

    
    void Start()
    {
        characterInput = CharacterInputManager.Instance;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeSelf)
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
        script.enabled = false;
        characterInput.enabled = false;
        Cursor.lockState = CursorLockMode.None;

    }
    public void Resume()
    {
        //Debug.Log("is work");
        pauseMenu.SetActive(false);
        script.enabled = true;
        characterInput.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Settings()
    {
        Debug.Log("Settings");
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
