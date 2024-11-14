using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UI;
using UI.Buttons.Gameplay;
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

        private GameMode _currentGameMode;

        /// Character Prefabs
        [Header("Character Prefabs")] 
        
        [SerializeField] private GameObject _roninPrefab;
        [SerializeField] private GameObject _shogunPrefab;
        [SerializeField] private GameObject _shinobiPrefab;
        
        /// Exposed Buttons
        [Header("Button Sets")]
        
        [SerializeField] private  GameplayButtonGroup _finishedButtons;
        
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
            _currentGameMode = gm;
            
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

        public void FinishGameMode()
        {
            _finishedButtons.ShowButtons();
        }

        public void ResetGameMode()
        {
            // Send Event to all characters to destroy themselves
            EventManager.Events.DestroyCharacters();
            // make a _finishedButtons.HideButtons(); method
            _finishedButtons.HideButtons();
        }

        #region Standoff

        private void SpawnCharacters()
        {
            Character p1 = null;
            
            // Player character
            switch (SaveManager.LoadPlayerCharacter())
            {
                case SamuraiType.Ronin:
                    p1 = InstantiateRonin(PlayerType.One);
                    break;
                case SamuraiType.Shogun:
                    p1 = InstantiateShogun(PlayerType.One);
                    break;
                case SamuraiType.Shinobi:
                    p1 = InstantiateShinobi(PlayerType.One);
                    break;
                case SamuraiType.Sensei:
                    break;
                case SamuraiType.Onna:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Opponent
            Character px = InstantiateShinobi(PlayerType.CPU);

            if (p1 is null || px is null) return;
            
            // Set as opponents
            p1.Opponent = px;
            px.Opponent = p1;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private Character InstantiateRonin(PlayerType playerType)
        { 
            var instance = Instantiate(_roninPrefab);
            Ronin ronin = instance.GetComponent<Ronin>();
            
            ronin.SetPlayerType(playerType);
            ronin.SetPosition();
            ronin.EndPosition = (playerType == PlayerType.One
                ? InitParams.Standoff_P1_EndPositionX
                : InitParams.Standoff_PX_EndPositionX);
            ronin.SetDirection();

            return ronin;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private Character InstantiateShogun(PlayerType playerType)
        {
            var instance = Instantiate(_shogunPrefab); 
            Shogun shogun = instance.GetComponent<Shogun>();
            
            shogun.SetPlayerType(playerType);
            shogun.SetPosition();
            shogun.EndPosition = (playerType == PlayerType.One
                ? InitParams.Standoff_P1_EndPositionX
                : InitParams.Standoff_PX_EndPositionX);
            shogun.SetDirection();

            return shogun;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private Character InstantiateShinobi(PlayerType playerType)
        {
            var instance = Instantiate(_shinobiPrefab);
            Shinobi shinobi = instance.GetComponent<Shinobi>();
            
            shinobi.SetPlayerType(playerType);
            shinobi.SetPosition();
            shinobi.EndPosition = (playerType == PlayerType.One
                ? InitParams.Standoff_P1_EndPositionX
                : InitParams.Standoff_PX_EndPositionX);
            shinobi.SetDirection();

            return shinobi;
        }

        private IEnumerator HaikuCountdown(IReadOnlyCollection<Haiku> haikus)
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