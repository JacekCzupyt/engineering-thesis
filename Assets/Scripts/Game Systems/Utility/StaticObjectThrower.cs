using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game_Systems.Utility {
    public static class ObjectThrower {
        public enum ThrowType {
            Static,
            Relative,
            AimAssist,
            Enhanced
        }

        public delegate GameObject ThrowMethod(
            Transform source,
            Vector3 direction,
            GameObject prefab,
            float velocity
        );

        public static ThrowMethod Throw(ThrowType type) {
            switch (type) {
                case ThrowType.Static:
                    return StaticThrow;
                case ThrowType.Relative:
                    throw new NotImplementedException();
                case ThrowType.AimAssist:
                    throw new NotImplementedException();
                case ThrowType.Enhanced:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static GameObject StaticThrow(
            Transform source,
            Vector3 direction,
            GameObject prefab,
            float velocity
        ) {
            var obj = CreateObject(source, prefab);
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddForce(direction.normalized * velocity, ForceMode.VelocityChange);
            }
            return obj;
        }

        private static GameObject CreateObject(Transform source, GameObject prefab) {
            return Object.Instantiate(prefab, source.position, source.rotation, SceneManager.Instance.ProjectileContainer.transform);
        }
    }
}
