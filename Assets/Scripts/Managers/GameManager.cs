using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using UI;
using UI.Buttons.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Random = UnityEngine.Random;

namespace Managers
{
    public partial class GameManager : MonoBehaviour
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

        private HaikuControls _haikuControls;
        private int i = 0;

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
            _haikuControls = new HaikuControls();
            _haikuControls.Player.Scroll.performed += OnHaikuScroll;

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
            EventManager.Events.RestartCurrentGameMode();
            _finishedButtons.HideButtons();
            StartGameMode(_currentGameMode);
        }

        #region Standoff

        private bool _standoffWinnerDeclared;
        private float _battleStartTime;

        private void SpawnCharacters()
        {
            // Player character
            var prefab = SelectSamuraiPrefab(SaveManager.LoadPlayerCharacter());
            Character p1 = 
                InstantiateCharacter(prefab, Character.PlayerNumber.One);

            // Opponent
            Character px = InstantiateCharacter(_roninPrefab, Character.PlayerNumber.CPU);

            if (p1 is null || px is null) return;
            
            // Set as opponents
            p1.Opponent = px;
            px.Opponent = p1;
        }

        /// Returns a GameObject prefab of the passed SamuraiType
        private GameObject SelectSamuraiPrefab(SamuraiType type)
        {
            switch (type)
            {
                case SamuraiType.Ronin: return _roninPrefab;
                case SamuraiType.Shogun: return _shogunPrefab;
                case SamuraiType.Shinobi: return _shinobiPrefab;
                case SamuraiType.Sensei:
                    break;
                case SamuraiType.Onna:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return null;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        /// Instantiates a Character in the scene from the GameManager's prefab list.
        private static Character InstantiateCharacter(GameObject prefab, 
            Character.PlayerNumber playerNumber)
        {
            // Create instance
            Character instance = Instantiate(prefab).GetComponent<Character>();
            instance.Player = playerNumber;
            
            // Positioning, directions, and controls
            instance.SetPosition();
            instance.EndPosition = (playerNumber == Character.PlayerNumber.One
                ? InitParams.Standoff_P1_EndPositionX
                : InitParams.Standoff_PX_EndPositionX);
            instance.SetDirection();
            instance.RegisterControls();

            return instance;
        }


        private IEnumerator HaikuCountdown(IReadOnlyCollection<Haiku> haikus)
        {
            
            // Fade out Haiku Text before starting (if applicable)
            if (!HaikuText.Instance.Hidden)
            {
                yield return HaikuText.Instance.FadeText(1f, AnimationDirection.Out);
            }
            
            // Reset Standoff Winner info
            _standoffWinnerDeclared = false;

            // Initial startup buffer
            yield return new WaitForSeconds(2.5f);

            // Choose haiku
            Haiku haiku = _haikus[Random.Range(0, haikus.Count)];

            int stage = 0;

            WaitForSeconds awaitStage1 = new WaitForSeconds(_timeUntilStage1);
            WaitForSeconds awaitStage2 = new WaitForSeconds(_timeUntilStage2);
            WaitForSeconds awaitStage3 = new WaitForSeconds(_timeUntilStage3);
            WaitForSeconds holdText = new WaitForSeconds(_timeHoldText);
            WaitForSeconds awaitBattle = new WaitForSeconds(_timeUntilBattleStart);
            
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeInDuration, 
                AnimationDirection.In));
            // Enable Haiku Selection
            _haikuControls.Enable();
            
            // // Line 1
            // yield return ExecuteStage(haiku.one[stage], stage++, 
            // awaitStage1, holdText);
            //
            // // Line 2
            // yield return ExecuteStage(haiku.two[stage], stage++, awaitStage2, 
            // holdText);
            //
            // // Line 3
            // yield return ExecuteStage(haiku.three[stage], stage++, awaitStage3, 
            // holdText);
            //
            // // Battle Start
            // HaikuText.Instance.SetTexts(new LinePair("Strike!", "攻撃！"));
            // yield return awaitBattle;
            // EventManager.Events.StageX(stage);
            // _battleStartTime = Time.time;
            // yield return StartCoroutine(
            //     HaikuText.Instance.FadeText(0.05f, AnimationDirection.In));
        }

        private IEnumerator ExecuteStage(
            LinePair text, int stage, WaitForSeconds awaitTime, WaitForSeconds holdTime)
        {
            // Set the HaikuText to the current line in the haiku
            HaikuText.Instance.SetTexts(text);
            
            // Wait a bit before showing the lines and starting the stage
            yield return awaitTime;
            
            // Broadcast to all observers that this stage is starting
            EventManager.Events.StageX(stage);
            
            // Fade text in
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeInDuration, 
                AnimationDirection.In));
            
            // Hold the text for a bit
            yield return holdTime;
            
            // Fade the text out
            yield return StartCoroutine(
                HaikuText.Instance.FadeText(_fadeOutDuration, AnimationDirection.Out));
            
        }

        /// Returns information about the reaction time and winner status
        /// after a Character inputs attack.
        public ReactionInfo AttackInput(float inputTime)
        {
            float reactionTime = inputTime - _battleStartTime;
            string formattedReactionTime = $"{reactionTime:F3}";

            // Loser
            if (_standoffWinnerDeclared)
                return new ReactionInfo(false, formattedReactionTime);
            
            // Winner
            _standoffWinnerDeclared = true;
            return new ReactionInfo(true, formattedReactionTime);
        }
        
        /// Fires when the Player scrolls left or right while the
        /// HaikuControls action map is enabled
        private void OnHaikuScroll(InputAction.CallbackContext obj)
        {
            // +1 for right, -1 for left
            int direction = (int) obj.action.ReadValue<Vector2>().x;

            switch (direction)
            {
                // Handle right
                case 1:
                    HaikuText.Instance.SetTexts(_haikus[0].one[(i++) % 3]);
                    break;
                // Handle left
                case -1:
                    HaikuText.Instance.SetTexts(_haikus[0].one[(i--) % 3]);
                    break;
            }
        }


        #endregion
    }
}