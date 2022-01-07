using System;
using System.Diagnostics;
using System.IO;
using PlasticGui.WorkspaceWindow;
using UnityEngine;
using UnityEngine.Windows;
using Debug = UnityEngine.Debug;
using File = System.IO.File;

namespace Game_Systems.Settings {
    public class SettingsManager : MonoBehaviour {
        
        public SettingsData data = SettingsData.Default;
        public string settingsFilePath = "Settings.dat";
        
        private static SettingsManager _instance;
        
        public static SettingsManager Instance {
            get {
                return _instance;
            }
        }
        
        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
            }
            DontDestroyOnLoad(this.gameObject);
            TryLoadSettings();
        }

        private void OnDestroy() {
            if(this == Instance)
                SaveSettings();
        }

        public void TryLoadSettings() {
            string json;
            try {
                json = File.ReadAllText(settingsFilePath);
            }
            catch (FileNotFoundException) {
                Debug.Log("Settings file not found");
                return;
            }
            catch (Exception e) {
                Debug.LogWarning($"Failed to load settings file.\n {e.Message}");
                return;
            }
            
            var sd = new SettingsData();
            try {
                sd.LoadFromJson(json);
            }
            catch(Exception e) {
                Debug.LogWarning($"Failed to interpret settings file.\n{e}");
                return;
            }

            data = sd;
            Debug.Log("Settings file loaded successfully");
        }

        public void SaveSettings() {
            try {
                var json = data.ToJson();
                File.WriteAllText(settingsFilePath, json);
                Debug.Log("Settings file saved successfully");
            }
            catch (Exception e) {
                Debug.LogWarning($"Failed to save settings.\n{e}");
            }
        }
    }
}
