using System.Collections;
using Characters;
using TMPro;
using UnityEngine;
using Util;

// ReSharper disable InconsistentNaming

namespace UI
{
    public class HaikuText : MonoBehaviour
    {
        public static HaikuText Instance { get; private set; }

        [Header("Child References")] [SerializeField]
        private TextMeshProUGUI _en, _jp;
        
        public bool Hidden { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            Hidden = false;
            
            _en.text = "";
            _jp.text = "";
            _en.alpha = 0;
            _jp.alpha = 0;
        }

        public IEnumerator FadeText(float fadeDuration, AnimationDirection animDirection)
        {
            float startAlpha = animDirection == AnimationDirection.In ? 0f : 1f;
            float endAlpha = animDirection == AnimationDirection.In ? 1f : 0f;
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

            Hidden = animDirection == AnimationDirection.Out;
        }

        public void SetTexts(LinePair lp)
        {
            _en.text = lp.en;
            _jp.text = lp.jp;
        }
    }
}