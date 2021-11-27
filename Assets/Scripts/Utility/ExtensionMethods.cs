using System.Linq;

namespace Utility {
    public static class ExtensionMethods
    {
        public static bool In<T>(this T x, params T[] set) {
            return set.Contains(x);
        }
    }
}
