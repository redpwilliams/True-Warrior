using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UI;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;
using PlayerType = Characters.Character.PlayerType;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        /// Singleton Instance
        public static GameManager Manager { get; private set; }

        /// GameModes that True Warrior offers
        public enum GameMode
        {
            Standoff,
            Survival,
            Zen
        }

        /// Character Prefabs
        [Header("Character Prefabs")] 
        
        [SerializeField] private GameObject _roninPrefab;
        [SerializeField] private GameObject _shogunPrefab;
        [SerializeField] private GameObject _shinobiPrefab;
        
        /// List of all haikus defined in "haiku.json"
        private List<Haiku> _haikus;

        [Header("Standoff Parameters")]
        
        [SerializeField] private float _timeUntilStage1 = 2.5f;
        [SerializeField] private float _timeUntilStage2 = 5f;
        [SerializeField] private float _timeUntilStage3 = 5f;

        // TODO - Make range?
        [SerializeField] private float _timeUntilBattleStart = 5f;
        [SerializeField] private float _fadeInDuration = 1f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _timeHoldText = 3.0f;


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
                    SpawnCharacters();
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

        private void SpawnCharacters()
        {
            // Player character
            switch (SaveManager.LoadPlayerCharacter())
            {
                case SamuraiType.Ronin:
                    InstantiateRonin(PlayerType.One);
                    break;
                case SamuraiType.Shogun:
                    InstantiateShogun(PlayerType.One);
                    break;
                case SamuraiType.Shinobi:
                    InstantiateShinobi(PlayerType.One);
                    break;
                case SamuraiType.Sensei:
                    break;
                case SamuraiType.Onna:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Opponent
            InstantiateShinobi(PlayerType.CPU);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void InstantiateRonin(PlayerType playerType)
        { 
            var instance = Instantiate(_roninPrefab);
            Ronin ronin = instance.GetComponent<Ronin>();
            
            ronin.SetPlayerType(playerType);
            ronin.SetPosition();
            ronin.EndPosition = (playerType == PlayerType.One
                ? InitParams.Standoff_P1_EndPositionX
                : InitParams.Standoff_PX_EndPositionX);
            ronin.SetDirection();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void InstantiateShogun(PlayerType playerType)
        {
            var instance = Instantiate(_shogunPrefab); 
            Shogun shogun = instance.GetComponent<Shogun>();
            
            shogun.SetPlayerType(playerType);
            shogun.SetPosition();
            shogun.EndPosition = (playerType == PlayerType.One
                ? InitParams.Standoff_P1_EndPositionX
                : InitParams.Standoff_PX_EndPositionX);
            shogun.SetDirection();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void InstantiateShinobi(PlayerType playerType)
        {
            var instance = Instantiate(_shinobiPrefab);
            Shinobi shinobi = instance.GetComponent<Shinobi>();
            
            shinobi.SetPlayerType(playerType);
            shinobi.SetPosition();
            shinobi.EndPosition = (playerType == PlayerType.One
                ? InitParams.Standoff_P1_EndPositionX
                : InitParams.Standoff_PX_EndPositionX);
            shinobi.SetDirection();
        }

        private IEnumerator HaikuCountdown(IReadOnlyList<Haiku> haikus)
        {
            // Initial startup buffer
            yield return new WaitForSeconds(2.5f);

            // Choose haiku
            Haiku haiku = _haikus[Random.Range(0, haikus.Count)];

            int stage = 0;

            // TODO - Create all WaitForSeconds objects here
            WaitForSeconds awaitStage1 = new WaitForSeconds(_timeUntilStage1);
            WaitForSeconds awaitStage2 = new WaitForSeconds(_timeUntilStage2);
            WaitForSeconds awaitStage3 = new WaitForSeconds(_timeUntilStage3);
            WaitForSeconds holdText = new WaitForSeconds(_timeHoldText);
            WaitForSeconds awaitBattle = new WaitForSeconds(_timeUntilBattleStart);
            
            // Line 1
            HaikuText.Instance.SetTexts(haiku.lines[stage]);
            yield return awaitStage1;
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeInDuration, true));
            yield return holdText;
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeOutDuration, false));

            // Line 2
            HaikuText.Instance.SetTexts(haiku.lines[stage]);
            yield return awaitStage2;
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeInDuration, true));
            yield return holdText;
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeOutDuration, false));

            // Line 3
            HaikuText.Instance.SetTexts(haiku.lines[stage]);
            yield return awaitStage3;
            EventManager.Events.StageX(stage++);
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeInDuration, true));
            yield return holdText;
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeOutDuration, false));

            // Battle Start
            HaikuText.Instance.SetTexts(new LinePair("Strike!", "攻撃！"));
            yield return awaitBattle;
            EventManager.Events.StageX(stage);
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(0.05f, true));
        }

        #endregion
    }
}