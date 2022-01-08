using System;
using System.Globalization;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings {
    public class SettingSlider : MonoBehaviour {
        private Slider slider;
        private InputField input;
        private InputField Input {
            get {
                if (input == null) {
                    input = GetComponentInChildren<InputField>();
                    input.onValueChanged.AddListener(InputValueChanged);
                    input.onEndEdit.AddListener(ResetText);
                }
                return input;
            }
        }

        private Slider Slider {
            get {
                if (slider == null) {
                    slider = GetComponentInChildren<Slider>();
                    slider.minValue = minVal;
                    slider.maxValue = maxVal;
                    slider.onValueChanged.AddListener(SliderValueChanged);
                }
                return slider;
            }
        }

        [SerializeField] public float minVal;
        [SerializeField] public float maxVal;

        private float value;

        // private void Awake() {
        //     
        //
        //     
        // }

        private void InputValueChanged(string s) {
            try {
                var val = float.Parse(s, CultureInfo.InvariantCulture);
                value = Mathf.Clamp(val, minVal, maxVal);
                Slider.value = value;
                ValueChanged?.Invoke(value);
            }
            catch {
                Debug.LogWarning("Could not parse string");
            }
        }

        private void SliderValueChanged(float val) {
            value = val;
            if(!Input.isFocused)
                Input.text = val.ToString(CultureInfo.InvariantCulture);
            ValueChanged?.Invoke(val);
        }

        public event Action<float> ValueChanged;

        private void ResetText(string _) {
            Input.text = value.ToString(CultureInfo.InvariantCulture);;
        }

        public void Init(float val) {
            value = val;
            Input.text = val.ToString(CultureInfo.InvariantCulture);
            Slider.value = val;
        }
    }
}
