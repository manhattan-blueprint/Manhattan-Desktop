using System;
using System.Collections.Generic;

namespace Utils {
    public static class Utils {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T, int> action){
            var i = 0;
            foreach (var element in enumerable) action(element, i++);
        }
    }
}