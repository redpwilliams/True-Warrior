using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        /// Singleton Instance
        public static GameManager Manager { get; private set; }

        public enum GameMode
        {
            Standoff,
            Survival,
            Zen
        }

        /// List of all haikus defined in "haiku.json"
        private List<Haiku> _haikus;

        [Header("Standoff Parameters")] [SerializeField]
        private float _timeUntilStage1 = 2.5f;

        [SerializeField] private float _timeUntilStage2 = 5f;

        [SerializeField] private float _timeUntilStage3 = 5f;

        // TODO - Make range?
        [SerializeField] private float _timeUntilBattleStart = 5f;
        [SerializeField] private float _fadeInDuration = 1f;
        [SerializeField] private float _fadeOutDuration = 0.5f;


        private void Awake()
        {
            if (Manager != null && Manager != this)
            {
                Destroy(Manager);
                return;
            }

            Manager = this;

            // Load haiku data
            _haikus = JsonReader.LoadHaikus();

            // Set up Standoff game mode parameters
            DontDestroyOnLoad(gameObject);
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

        #region Standoff

        private IEnumerator HaikuCountdown(IReadOnlyList<Haiku> haikus)
        {
            // Initial startup buffer
            yield return new WaitForSeconds(2.5f);

            // Choose haiku
            Haiku haiku = _haikus[Random.Range(0, haikus.Count)];

            int stage = 0;

            // TODO - Create all WaitForSeconds objects here

            // Line 1
            HaikuText.Instance.SetTexts(haiku.lines[stage]);
            yield return new WaitForSeconds(_timeUntilStage1);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(HaikuText.Instance.FadeText
            (_fadeInDuration,
                true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeOutDuration, false));

            // Line 2
            HaikuText.Instance.SetTexts(haiku.lines[stage]);
            yield return new WaitForSeconds(_timeUntilStage2);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(HaikuText.Instance.FadeText
            (_fadeInDuration,
                true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(HaikuText.Instance.FadeText
            (_fadeOutDuration,
                false));

            // Line 3
            HaikuText.Instance.SetTexts(haiku.lines[stage]);
            yield return new WaitForSeconds(_timeUntilStage3);
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeInDuration, true));
            yield return new WaitForSeconds(3f); // TODO - Time to hold text
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeOutDuration, false));

            // Battle Start
            HaikuText.Instance.SetTexts(new LinePair("Strike!", "攻撃！"));
            yield return new WaitForSeconds(_timeUntilBattleStart);
            EventManager.Events.StageX(stage);
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(0.05f, true));
            // TODO - Change text to different formatted text object?
        }

        #endregion
    }
}