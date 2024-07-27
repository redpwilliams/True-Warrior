using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Characters
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Lerp2D
    {
        // https://easings.net/
        public static float EaseInSine(float t) =>
            1 - Mathf.Cos((t * Mathf.PI) / 2);

        public static float EaseOutSine(float t) =>
            1 - Mathf.Sin((t * Mathf.PI) / 2);

        public static float EaseInOutSine(float t) =>
            -(Mathf.Cos(Mathf.PI * t) - 1) / 2;

        public static float EaseInQuad(float t) => Mathf.Pow(t, 2);
        public static float EaseOutQuad(float t) => 1 - (1 - t) * (1 - t);

        public static float EaseInOutQuad(float t) => t < 0.5f
            ? 2 * Mathf
                .Pow(t, 2)
            : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;

        public static float EaseInCubic(float t) => Mathf.Pow(t, 3);
        public static float EaseOutCubic(float t) => 1 - Mathf.Pow(1 - t, 3);

        public static float EaseInOutCubic(float t) => t < 0.5f
            ? 4 * Mathf.Pow(t, 3)
            : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;

        public static float EaseInQuart(float t) => Mathf.Pow(t, 4);
        public static float EaseOutQuart(float t) => 1 - Mathf.Pow(1 - t, 4);

        public static float EaseInOutQuart(float t) => t < 0.5f
            ? 8 * Mathf
                .Pow(t, 4)
            : 1
              - Mathf.Pow(-2 * t + 2, 4) / 2;

        public static float EaseInQuint(float t) => Mathf.Pow(t, 5);
        public static float EaseOutQuint(float t) => 1 - Mathf.Pow(1 - t, 5);

        public static float EaseInOutQuint(float t) => t < 0.5f
            ? 16 * Mathf
                .Pow(t, 5)
            : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;

        public static float EaseInExpo(float t) => Equals(t, 0)
            ? 0
            : Mathf.Pow(2, 10 * t - 10);

        public static float EaseOutExpo(float t) => Equals(t, 1)
            ? 1
            : 1 - Mathf.Pow(2, -10 * t);

        public static float EaseInOutExpo(float t) => Equals(t, 0)
            ? 0
            : Equals(t, 1)
                ? 1
                : t < 0.5
                    ? Mathf.Pow(2, 20 * t - 10) / 2
                    : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;

        public static float EaseInCirc(float t) =>
            1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));

        public static float EaseOutCirc(float t) => Mathf.Sqrt(1 - Mathf.Pow(t
            - 1, 2));

        public static float EaseInOutCirc(float t) => t < 0.5f
            ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2
            : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;

        public static float EaseInBack(float t)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;
            return c3 * Mathf.Pow(t, 3) - c1 * Mathf.Pow(t, 2);
        }

        public static float EaseOutBack(float t)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;
            return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
        }

        public static float EaseInOutBack(float t)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;

            return t < 0.5
                ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
                : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) +
                                              c2) + 2) / 2;
        }


        public static float EaseInElastic(float t)
        {
            float c4 = (2 * Mathf.PI) / 3;

            return Equals(t, 0f)
                ? 0
                : Equals(t, 1f)
                    ? 1
                    : -Mathf.Pow(2, 10 * t - 10) *
                      Mathf.Sin((t * 10 - 10.75f) * c4);
        }

        public static float EaseOutElastic(float t)
        {
            float c4 = (2 * Mathf.PI) / 3;
            return Equals(t, 0f)
                ? 0
                : Equals(t, 1f)
                    ? 1
                    : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) +
                      1;
        }

        public static float EaseInOutElastic(float t)
        {
            float c5 = (2 * Mathf.PI) / 4.5f;

            return Equals(t, 0f)
                ? 0
                : Equals(t, 1f)
                    ? 1
                    : t < 0.5
                        ? -(Mathf.Pow(2, 20 * t - 10) *
                            Mathf.Sin((20 * t - 11.125f) * c5)) / 2
                        : (Mathf.Pow(2, -20 * t + 10) *
                           Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
        }

        public static float EaseInBounce(float t) => 1 - EaseOutBounce(1 - t);

        public static float EaseOutBounce(float t)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (t < 1 / d1)
                return n1 * Mathf.Pow(t, 2);
            if (t < 2 / d1)
                return n1 * (t -= 1.5f / d1) * t + 0.75f;
            if (t < 2.5 / d1)
                return n1 * (t -= 2.25f / d1) * t + 0.9375f;

            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }

        public static float EaseInOutBounce(float t)
        {
            return t < 0.5f
                ? (1 - EaseOutBounce(1 - 2 * t)) / 2
                : (1 + EaseOutBounce(2 * t - 1)) / 2;
        }

        private static bool Equals(float t, float val)
        {
            float tolerance = 0.001f;
            return Mathf.Abs(t - val) < tolerance;
        }
    }
}