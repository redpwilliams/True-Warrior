using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable InconsistentNaming

namespace UI
{
    public class HaikuText : MonoBehaviour
    {
        private List<JsonReader.Haiku> _haikus;

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
            _haikus = new JsonReader().Haikus;
        }

        public void StartGameMode(GameMode gm)
        {
            switch (gm)
            {
                case GameMode.Standoff: 
                    StartCoroutine(HaikuCountdown(_haikus));
                    break;
                case GameMode.Survival:
                    break;
                case GameMode.Zen:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gm), gm, null);
            }
        }

        private IEnumerator HaikuCountdown(List<JsonReader.Haiku> haikus)
        {
            // Initial startup buffer
            yield return new WaitForSeconds(2.5f);
        
            // Choose haiku
            JsonReader.Haiku haiku = haikus[Random.Range(0, haikus.Count)];
        
            int stage = 0;

            // Line 1
            SetTexts(haiku.lines[stage]);
            yield return new WaitForSeconds(timeUntilStage1);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(FadeText(fadeInDuration, true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
            // Line 2
            SetTexts(haiku.lines[stage]);
            yield return new WaitForSeconds(timeUntilStage2);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(FadeText(fadeInDuration, true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
            // Line 3
            SetTexts(haiku.lines[stage]);
            yield return new WaitForSeconds(timeUntilStage3);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(FadeText(fadeInDuration, true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(FadeText(fadeOutDuration, false));
        
            // Battle Start
            SetTexts(new JsonReader.LinePair("Strike!","攻撃！"));
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

        private void SetTexts(JsonReader.LinePair lp)
        {
            _en.text = lp.en;
            _jp.text = lp.jp;
        }

        private class JsonReader
        {
            public List<Haiku> Haikus { get; }

            public JsonReader()
            {
                string jsonString = Resources.Load<TextAsset>("haikus").ToString();
                Haikus = JsonUtility.FromJson<JsonData>(jsonString).haikus;
            }

            [Serializable]
            internal struct JsonData
            {
                public List<Haiku> haikus;
            }

            [Serializable]
            internal struct Haiku
            {
                public List<LinePair> lines;
            }

            [Serializable]
            internal struct LinePair
            {
                public string en;
                public string jp;

                public LinePair(string en, string jp)
                {
                    this.en = en;
                    this.jp = jp;
                }
            }
        }
    }
}