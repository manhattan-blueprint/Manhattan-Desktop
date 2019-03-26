using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Utils {
    public class ScreenProportions : MonoBehaviour {
        private float height;
        private float width;

        void Update() {
            height = Screen.height;
            width = Screen.width;
        }

        // Converts a number to that proportional height in the screen
        public float ToH(float dec) {
            return height * dec;
        }

        // Converts a number to that proportional width in the screen
        public float ToW(float dec) {
            return width * dec;
        }

        // Converts a vector full of decimals to the proportional width in the screen
        public Vector3 ToV(Vector3 decVector) {
            return new Vector3(decVector.x * width, decVector.y * height, 0.0f);
        }

        // Converts a vector full of decimals to the proportional width in the screen
        public Vector2 ToV(Vector2 decVector) {
            return new Vector3(decVector.x * width, decVector.y * height);
        }
    }
}
