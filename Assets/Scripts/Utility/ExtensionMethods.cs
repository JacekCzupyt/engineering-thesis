using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Utility {
    public static class ExtensionMethods {
        private static Random rng = new Random();

        public static bool In<T>(this T x, params T[] set) {
            return set.Contains(x);
        }

        public static bool In<T>(this T x, IEnumerable<T> list) {
            return list.Contains(x);
        }

        public static T RandomElement<T>(this IList<T> list) {
            return list[rng.Next(list.Count)];
        }

        public static float GetHorizontalFov(this Camera cam) {
            var radAngle = cam.fieldOfView * Mathf.Deg2Rad;
            var radHfov = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * cam.aspect);
            return Mathf.Rad2Deg * radHfov;
        }

        public static void SetHorizontalFov(this Camera cam, float hFov) {
            var radAngle = hFov * Mathf.Deg2Rad;
            var radHfov = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) / cam.aspect);
            cam.fieldOfView = Mathf.Rad2Deg * radHfov;
        }
    }
}
