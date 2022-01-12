using Game_Systems.Settings;
using UI.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace UI.MainMenu {
    public class SettingsMenuManager : MonoBehaviour {
        [SerializeField] public UnityEvent closeMenu;
        
        [SerializeField] private SettingSlider fov;
        [SerializeField] private SettingSlider sensitivity;
        [SerializeField] private SettingSlider roll;
        [SerializeField] private SettingSlider volume;
        [SerializeField] private SettingsCheckbox aimToggle;

        private SettingsManager settings;
        private SettingsManager Settings {
            get {
                if (settings == null)
                    settings = SettingsManager.Instance;
                return settings;
            }
        }

        private void Start() {
            InitializeCallbacks();
        }

        private void InitializeCallbacks() {
            fov.ValueChanged += v => Settings.data.fov = v;
            sensitivity.ValueChanged += v => Settings.data.mouseSensitivity = v;
            roll.ValueChanged += v => Settings.data.mouseRollSensitivity = v;
            volume.ValueChanged += v => Settings.data.volume = v / 100;
            aimToggle.ValueChanged += v => settings.data.aimToggle = v;
        }

        private void OnEnable() {
            Settings.BackupSettings();

            InitializeSliders();
        }

        private void InitializeSliders() {
            fov.Init(Settings.data.fov);
            sensitivity.Init(Settings.data.mouseSensitivity);
            roll.Init(Settings.data.mouseRollSensitivity);
            volume.Init(Settings.data.volume * 100);
            aimToggle.Init(settings.data.aimToggle);
        }

        public void AcceptButton() {
            settings.SaveSettings();
            closeMenu.Invoke();
        }

        public void CancelButton() {
            Settings.RestoreBackup();
            closeMenu.Invoke();
        }

        public void RevertToDefaultButton() {
            Settings.data = SettingsData.Default;
            InitializeSliders();
        }
    }
}
