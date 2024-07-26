using UnityEngine;

namespace Characters
{
    public static class Lerping2D
    {
        public static float EaseOutQuart(float t) => 1 - Mathf.Pow(1 - t, 4);
    }
}