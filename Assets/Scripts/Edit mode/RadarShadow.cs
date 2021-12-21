using System;
using UnityEngine;

namespace Edit_mode {
    [ExecuteInEditMode]
    public class RadarShadow : MonoBehaviour {
        [SerializeField] private LineRenderer line;
        [SerializeField] private Transform shadow;

        public void Update() {
            Vector3 relativePosition = new Vector3(0, -transform.localPosition.y, 0);
            if(line != null)
                line.SetPosition(1, relativePosition);
            if(shadow != null)
                shadow.localPosition = relativePosition;
        }
    }
}
