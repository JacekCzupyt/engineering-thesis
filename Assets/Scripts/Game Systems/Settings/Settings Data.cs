using System;
using UnityEngine;

namespace Game_Systems.Settings {
    [System.Serializable]
    public class SettingsData : IJsonSerializable {
        public float fov;
        public float mouseSensitivity;
        public float mouseRollSensitivity;
        public float volume;

        public string ToJson() {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string json) {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public static SettingsData Default => new SettingsData {
            fov = 91.5f,
            mouseSensitivity = 0.27f,
            mouseRollSensitivity = 0.5f,
            volume = 1
        };
    }

    public interface IJsonSerializable {
        public string ToJson();
        public void LoadFromJson(string json);
    }
}
