using System;
using System.Diagnostics;
using Game_Systems.Utility;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UI.Game {
    public class Crosshair : MonoBehaviour {
        [SerializeField] private RectTransform crosshairCenter;
        [SerializeField] private RectTransform crosshairLeft;
        [SerializeField] private RectTransform crosshairRight;
        [SerializeField] private RectTransform crosshairTop;
        [SerializeField] private RectTransform crosshairBottom;

        [SerializeField] private float minDistance;
        [SerializeField] private Camera cam;

        public void SetSpread(float distance) {
            var trueDistance = distance + minDistance;
            crosshairLeft.localPosition = new Vector3(-trueDistance, 0, 0);
            crosshairRight.localPosition = new Vector3(trueDistance, 0, 0);
            crosshairBottom.localPosition = new Vector3(0, -trueDistance, 0);
            crosshairTop.localPosition = new Vector3(0, trueDistance, 0);
        }

        public void SetSpreadFromAngle(float angle) {
            if (cam == null)
                throw new NullReferenceException("SetSpreadFromAngle can't be used if camera is not assigned");
            
            var screenSpacePoint = new Vector3(0, angle / cam.fieldOfView, 0).y * this.GetComponent<RectTransform>().rect.height;
            SetSpread(screenSpacePoint);
        }
    }
}
