using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings {
    public class SettingsCheckbox : MonoBehaviour {
        private Toggle toggle;

        private Toggle Toggle {
            get {
                if (toggle == null) {
                    toggle = GetComponentInChildren<Toggle>();
                    toggle.onValueChanged.AddListener(ToggleValueChanged);
                }
                return toggle;
            }
        }

        private bool value;

        private void ToggleValueChanged(bool val) {
            value = val;
            ValueChanged?.Invoke(val);
        }

        public event Action<bool> ValueChanged;

        public void Init(bool val) {
            value = val;
            Toggle.isOn = val;
        }
    }
}
