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

        public static int GetRandomInt(int amount){
            return rng.Next(1, amount+1);
        }
    }
}
