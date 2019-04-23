using System.Collections;
using UnityEngine;

namespace Utils {
    public enum Anim {
        MoveToAccelerate,
        MoveToDecelerate,
        OscillateHeight,
        Grow,
        Appear,
        Disappear,
        OscillateAlpha,
        OscillateSize
    }

    // Makes an animation happen to an object, and then removes the component
    // after. The component is therefore not removed if the animation is
    // indefinite.
    class ManhattanAnimation : MonoBehaviour {
        private static int overshootAmount = 110;
        private IEnumerator timedCoroutine;
        private float overshoot;
        private float framePeriod;

        ////////////////////////////////////////////////////////////////////////
        // Movement Animations.
        ////////////////////////////////////////////////////////////////////////
        public void StartMovementAnimation(GameObject inpObject,
            Anim anim, Vector3 moveVector, float time = 1.0f,
            bool destroyAfter = false, float delay = 0.0f) {
            ManhattanAnimation animation = inpObject.AddComponent<ManhattanAnimation>() as ManhattanAnimation;
            animation.StartMovementAnimation(anim, moveVector, time, destroyAfter, delay);
        }

        public void StartMovementAnimation(Anim anim, Vector3 moveVector,
            float time, bool destroyAfter, float delay) {
            timedCoroutine = null;

            // Equal to 1 / (1 + sin(20)) degrees, for overshooting then returning
            // to the desired position.
            overshoot = 1.0f / Mathf.Sin(overshootAmount * Mathf.Deg2Rad);

            // Equivalent to 1 / 60.
            framePeriod = 1.0f / 60.0f;

            switch (anim) {
                case Anim.MoveToAccelerate:
                    // timedCoroutine = MoveToAccelerate(moveVector, 0.1f, destroyAfter, delay);
                    break;

                case Anim.MoveToDecelerate:
                    timedCoroutine = MoveToDecelerate(moveVector, time, destroyAfter, delay);
                    break;

                case Anim.OscillateHeight:
                    timedCoroutine = OscillateHeight();
                    break;

                default:
                    break;
            }

            if (timedCoroutine != null) StartCoroutine(timedCoroutine);
        }

        private IEnumerator MoveToAccelerate(Vector3 moveVector, float time,
            bool destroyAfter, float delay) {
            float speed = (120.0f * framePeriod) / time;

            Vector3 originalPosition = gameObject.transform.position;

            yield return new WaitForSeconds(delay);

            for (float count = 90.0f; count < 210.0f; count += speed) {
                float speedModifier = 1.0f - Mathf.Sin(count * Mathf.Deg2Rad);

                gameObject.transform.position = originalPosition + overshoot * speedModifier * moveVector;

                yield return new WaitForSeconds(framePeriod);
            }

            gameObject.transform.position = originalPosition + moveVector;

            if (destroyAfter) MonoBehaviour.Destroy(gameObject);

            Destroy(this);
        }

        private IEnumerator MoveToDecelerate(Vector3 moveVector, float time,
            bool destroyAfter, float delay) {
            float speed = (120.0f * framePeriod) / time;

            Vector3 originalPosition = gameObject.transform.position;

            yield return new WaitForSeconds(delay);

            for (float count = 0; count <= overshootAmount; count += speed) {
                float speedModifier = Mathf.Sin(count * Mathf.Deg2Rad);
                
                gameObject.transform.position = originalPosition + overshoot * speedModifier * moveVector;

                yield return new WaitForSeconds(framePeriod);
            }

            gameObject.transform.position = originalPosition + moveVector;

            if (destroyAfter) MonoBehaviour.Destroy(gameObject);

            Destroy(this);
        }

        private IEnumerator OscillateHeight() {
            float maxDifference = 0.5f;

            for (int count = 0; ; count++) {
                yield return new WaitForSeconds(framePeriod);

                float positionModifier = maxDifference * Mathf.Sin((Mathf.PI * count) / 90) / 200;

                gameObject.transform.position += new Vector3(0.0f, positionModifier, 0.0f);
            }

            Destroy(this);
        }

        ////////////////////////////////////////////////////////////////////////
        // Appearance Animations.
        ////////////////////////////////////////////////////////////////////////
        public void StartAppearanceAnimation(GameObject inpObject,
            Anim anim, float time = 1.0f, bool boolModifier = false,
            float floatModifier = 200.0f, float delay = 0.0f) {
            ManhattanAnimation animation = inpObject.AddComponent<ManhattanAnimation>() as ManhattanAnimation;
            animation.StartAppearanceAnimation(anim, time, boolModifier, floatModifier, delay);
        }

        public void StartAppearanceAnimation(Anim anim, float time,
            bool boolModifier, float floatModifier, float delay) {
            timedCoroutine = null;

            // Equal to 1 / (1 + sin(20)) degrees, for overshooting then returning
            // to the desired position.
            overshoot = 1.0f / Mathf.Sin(120 * Mathf.Deg2Rad);

            // Equivalent to 1 / 60.
            framePeriod = 1.0f / 60.0f;

            switch (anim) {
                case Anim.OscillateAlpha:
                    timedCoroutine = OscillateAlpha(time, boolModifier, floatModifier, delay);
                    break;

                case Anim.OscillateSize:
                    timedCoroutine = OscillateSize(time, boolModifier, floatModifier, delay);
                    break;

                case Anim.Grow:
                    timedCoroutine = Grow(time, floatModifier, delay);
                    break;

                case Anim.Appear:
                    timedCoroutine = Appear(time, delay);
                    break;

                case Anim.Disappear:
                    timedCoroutine = Disappear(time, delay);
                    break;

                default:
                    break;
            }

            if (timedCoroutine != null) StartCoroutine(timedCoroutine);
        }

        private IEnumerator OscillateAlpha(float time, bool makeBounce,
            float minimumAlpha, float delay) {
            float speed = (framePeriod * 360) / time;

            yield return new WaitForSeconds(delay);

            for (float count = 0; ; count += speed) {
                yield return new WaitForSeconds(framePeriod);

                float alphaModifier = ((Mathf.Sin(count * Mathf.Deg2Rad) + 1) / 2.0f)
                    * (1 - minimumAlpha) + minimumAlpha;

                if (!makeBounce) alphaModifier = Mathf.Abs(alphaModifier);

                if (gameObject.GetComponent<CanvasGroup>() != null)
                    gameObject.GetComponent<CanvasGroup>().alpha = alphaModifier;
            }
            Destroy(this);
        }

        private IEnumerator OscillateSize(float time, bool makeBounce,
            float targetSize, float delay) {
            float speed = (framePeriod * 360) / time;

            Vector3 originalSize = gameObject.transform.localScale;

            yield return new WaitForSeconds(delay);

            for (float count = 0; ; count += speed) {
                yield return new WaitForSeconds(framePeriod);

                float size;

                if (!makeBounce)
                    size = 1 + (targetSize - 1) * overshoot * Mathf.Sin(count * Mathf.Deg2Rad);
                else
                    size = 1 + (targetSize - 1) * overshoot * Mathf.Abs(Mathf.Sin(count * Mathf.Deg2Rad));

                gameObject.transform.localScale = size * originalSize;
            }
            Destroy(this);
        }

        private IEnumerator Grow(float time, float targetSize, float delay) {
            float speed = (framePeriod * 120.0f) / time;

            Vector3 originalSize = gameObject.transform.localScale;

            yield return new WaitForSeconds(delay);

            for (float count = 0.0f; count < 120.0f; count += speed) {
                yield return new WaitForSeconds(framePeriod);

                float size = 1 + (targetSize - 1) * overshoot * Mathf.Sin(count * Mathf.Deg2Rad);

                gameObject.transform.localScale = size * originalSize;
            }
            gameObject.transform.localScale = targetSize * originalSize;

            Destroy(this);
        }

        private IEnumerator Appear(float time, float delay) {
            if (gameObject.GetComponent<CanvasGroup>() != null)
                gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
            if (gameObject.GetComponent<SpriteRenderer>() != null) {
                Color color = gameObject.GetComponent<SpriteRenderer>().color;
                color.a = 0f;
                gameObject.GetComponent<SpriteRenderer>().color = color;
            }
            if (gameObject.GetComponent<TextMesh>() != null) {
                Color color = gameObject.GetComponent<TextMesh>().color;
                color.a = 0f;
                gameObject.GetComponent<TextMesh>().color = color;
            }

            float speed = (framePeriod * 90.0f) / time;

            yield return new WaitForSeconds(delay);

            // Note: does not overshoot.
            for (float count = 0.0f; count < 90.0f; count += speed) {
                yield return new WaitForSeconds(framePeriod);

                float alphaModifier = (Mathf.Sin(count * Mathf.Deg2Rad));

                if (gameObject.GetComponent<CanvasGroup>() != null)
                    gameObject.GetComponent<CanvasGroup>().alpha = alphaModifier;
                if (gameObject.GetComponent<SpriteRenderer>() != null) {
                    Color color = gameObject.GetComponent<SpriteRenderer>().color;
                    color.a = alphaModifier;
                    gameObject.GetComponent<SpriteRenderer>().color = color;
                }
                if (gameObject.GetComponent<TextMesh>() != null) {
                    Color color = gameObject.GetComponent<TextMesh>().color;
                    color.a = alphaModifier;
                    gameObject.GetComponent<TextMesh>().color = color;
                }
            }
            Destroy(this);
        }

        private IEnumerator Disappear(float time, float delay) {
            float speed = (framePeriod * 90.0f) / time;

            yield return new WaitForSeconds(delay);

            // Note: does not overshoot.
            for (float count = 90.0f; count < 180.0f; count += speed) {
                yield return new WaitForSeconds(framePeriod);

                float alphaModifier = Mathf.Sin(count * Mathf.Deg2Rad);

                if (gameObject.GetComponent<CanvasGroup>() != null)
                    gameObject.GetComponent<CanvasGroup>().alpha = alphaModifier;
            }
            Destroy(this);
        }
    }
}
