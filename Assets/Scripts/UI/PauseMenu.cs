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

    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        script.enabled = true;
    }
    public void LeaveGame()
    {
        if (IsOwner)
        {
            NetworkManager.Singleton.StopClient();

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
