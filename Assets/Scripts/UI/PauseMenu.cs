using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
public class PauseMenu : NetworkBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Behaviour script;

    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    void Pause()
    {
        pauseMenu.SetActive(true);
        script.enabled = false;
        Input_Systems.CharacterInputManager.Instance.enabled = false;
        Cursor.lockState = CursorLockMode.None;

    }
    public void Resume()
    {
        Debug.Log("is work");
        pauseMenu.SetActive(false);
        script.enabled = true;
        Input_Systems.CharacterInputManager.Instance.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LeaveGame()
    {
        if (IsOwner)
        {
            NetworkManager.Singleton.StopClient();
            Input_Systems.CharacterInputManager.Instance.enabled = true;
        }
    }
    public void QuitGame()
    {
        if(IsOwner)
        {
            NetworkManager.Singleton.StopClient();
            Application.Quit();
        }
    }
}
