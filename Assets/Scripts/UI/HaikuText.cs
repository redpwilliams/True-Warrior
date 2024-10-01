using System.Collections;
using Characters;
using TMPro;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace UI
{
    public class HaikuText : MonoBehaviour
    {
        public static HaikuText Instance { get; private set; }

        [Header("Child References")] [SerializeField]
        private TextMeshProUGUI _en, _jp;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            _en.text = "";
            _jp.text = "";
            _en.alpha = 0;
            _jp.alpha = 0;
        }

        public IEnumerator FadeText(float fadeDuration, bool fadeIn)
        {
            float startAlpha = fadeIn ? 0f : 1f;
            float endAlpha = fadeIn ? 1f : 0f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime /
                    fadeDuration);
                _en.alpha = alpha;
                _jp.alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _en.alpha = endAlpha;
            _jp.alpha = endAlpha;
        }

        public void SetTexts(LinePair lp)
        {
            _en.text = lp.en;
            _jp.text = lp.jp;
        }
    }
}