using System;
using System.Collections.Generic;

namespace Utils {
    public static class Utils {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T, int> action) {
            int i = 0;
            foreach (T element in enumerable) action(element, i++);
        }
    }
}
