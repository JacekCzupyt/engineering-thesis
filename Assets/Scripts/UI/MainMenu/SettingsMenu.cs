using UnityEngine;

namespace UI.MainMenu {
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPrefab;
        [SerializeField] private GameObject settingsContainer;
        private GameObject settingsMenu;
        public SettingsMenu(string name, GameObject settings)
        {
            settingsMenu = Instantiate(settingsPrefab) as GameObject;
            settingsMenu.transform.SetParent(settingsContainer.transform);
            settingsMenu.SetActive(true);
        }
    }
}
