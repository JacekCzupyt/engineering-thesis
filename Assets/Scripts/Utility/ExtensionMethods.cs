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

        public static T RandomElement<T>(this IList<T> list) {
            return list[rng.Next(list.Count)];
        }
    }
}
