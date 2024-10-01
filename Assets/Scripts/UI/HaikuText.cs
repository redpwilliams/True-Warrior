using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using GameMode = Managers.GameManager.GameMode;

// ReSharper disable InconsistentNaming

namespace UI
{
    public class HaikuText : MonoBehaviour
    {
        private List<Haiku> _haikus;

        [Header("Child References")] [SerializeField]
        private TextMeshProUGUI _en, _jp;

        [Header("Standoff Parameters")]
        [SerializeField] private float timeUntilStage1= 2.5f;
        [SerializeField] private float timeUntilStage2 = 5f;
        [SerializeField] private float timeUntilStage3 = 5f;
        // TODO - Make range?
        [SerializeField] private float timeUntilBattleStart = 5f;
        [SerializeField] private float fadeInDuration = 1f;
        [SerializeField] private float fadeOutDuration = 0.5f;

        private void Awake()
        {
            _en.text = "";
            _jp.text = "";
            _en.alpha = 0;
            _jp.alpha = 0;
            _haikus = new List<Haiku>();
        }


        public IEnumerator HaikuCountdown(IReadOnlyList<Haiku> haikus)
        {
            // Initial startup buffer
            yield return new WaitForSeconds(2.5f);
        
            // Choose haiku
            Haiku haiku = haikus[Random.Range(0, haikus.Count)];
        
            int stage = 0;

            // Line 1
            SetTexts(haiku.Lines[stage]);
            yield return new WaitForSeconds(timeUntilStage1);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(FadeText(fadeInDuration, true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
            // Line 2
            SetTexts(haiku.Lines[stage]);
            yield return new WaitForSeconds(timeUntilStage2);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(FadeText(fadeInDuration, true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
            // Line 3
            SetTexts(haiku.Lines[stage]);
            yield return new WaitForSeconds(timeUntilStage3);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(FadeText(fadeInDuration, true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
            // Battle Start
            SetTexts(new LinePair("Strike!","攻撃！"));
            yield return new WaitForSeconds(timeUntilBattleStart);
            EventManager.Events.StageX(stage);
            yield return StartCoroutine(FadeText(0.05f, true));
            // TODO - Change text to different formatted text object?
        }
    
        private IEnumerator FadeText(float fadeDuration, bool fadeIn)
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

        private void SetTexts(LinePair lp)
        {
            _en.text = lp.En;
            _jp.text = lp.Jp;
        }

    }
}