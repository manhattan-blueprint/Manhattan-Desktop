using System;
using System.Collections.Generic;
using Controller;
using UnityEngine;

namespace Utils {
    public static class Utils {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T, int> action) {
            int i = 0;
            foreach (T element in enumerable) action(element, i++);
        }

        public static void ShowAlert(this MonoBehaviour behaviour, string title, string message) {
            GameObject.FindWithTag("Alert").GetComponent<AlertController>().SetAlert(title, message);
            GameObject.FindWithTag("Alert").GetComponent<AlertController>().ShowAlertView();
        }
    }
}
