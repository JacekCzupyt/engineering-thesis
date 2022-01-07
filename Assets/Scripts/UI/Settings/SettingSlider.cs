using System;
using System.Globalization;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings {
    public class SettingSlider : MonoBehaviour {
        private Slider slider;
        private InputField input;

        [SerializeField] public float minVal;
        [SerializeField] public float maxVal;

        private float value;

        private void Awake() {
            slider = GetComponentInChildren<Slider>();
            slider.minValue = minVal;
            slider.maxValue = maxVal;
            slider.onValueChanged.AddListener(SliderValueChanged);

            input = GetComponentInChildren<InputField>();
            input.onValueChanged.AddListener(InputValueChanged);
            input.onEndEdit.AddListener(ResetText);
        }

        private void InputValueChanged(string s) {
            try {
                var val = float.Parse(s, CultureInfo.InvariantCulture);
                value = Mathf.Clamp(val, minVal, maxVal);
                slider.value = value;
                ValueChanged?.Invoke(value);
            }
            catch {
                Debug.LogWarning("Could not parse string");
            }
        }

        private void SliderValueChanged(float val) {
            value = val;
            if(!input.isFocused)
                input.text = val.ToString(CultureInfo.InvariantCulture);
            ValueChanged?.Invoke(val);
        }

        public event Action<float> ValueChanged;

        private void ResetText(string _) {
            input.text = value.ToString(CultureInfo.InvariantCulture);;
        }

        public void Init(float val) {
            value = val;
            input.text = val.ToString(CultureInfo.InvariantCulture);
            slider.value = val;
        }
    }
}
