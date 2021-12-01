using UnityEngine;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject initialMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject settingsMenu;

    public void Play(){
        SwitchMenus(initialMenu, playMenu);
    }

    public void Exit(){
        Application.Quit();
    }

    public void Settings(){
        SwitchMenus(initialMenu, settingsMenu);
    }
    
    public void BackToInitialMenu(int menuIndex){
        switch(menuIndex)
        {
            case 0: SwitchMenus(playMenu, initialMenu);
            break;

            case 1: SwitchMenus(settingsMenu, initialMenu);
            break;

            default: 
            break;
        }
    }

    private void SwitchMenus(GameObject disable, GameObject enable)
    {
        disable.SetActive(false);
        enable.SetActive(true);
    }

    public void DisableSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void EnableSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }
}
