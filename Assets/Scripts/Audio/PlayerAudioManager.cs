using Game_Systems.Settings;
using UnityEngine;

namespace Audio {
    public class PlayerAudioManager : MonoBehaviour {
        private SettingsManager settings;
        private void Start() {
            settings = SettingsManager.Instance;
        }

        private void Update() {
            AudioListener.volume = settings.data.volume;
        }
    }
}
