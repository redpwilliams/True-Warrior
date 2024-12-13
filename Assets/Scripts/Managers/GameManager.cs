using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using ScriptableObjects;
using UI;
using UI.Buttons.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Util;
using Random = UnityEngine.Random;
using PlayerNumber = Characters.PlayerNumber;

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
        private float _gameModeStartupBuffer;

        /// Exposed Buttons
        [Header("Button Sets")]
        
        [SerializeField] private  GameplayButtonGroup _finishedButtons;
        
        /// List of all haikus defined in "haiku.json"
        private List<Haiku> _haikus;

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

            _gameModeStartupBuffer = 2.5f;

            DontDestroyOnLoad(gameObject);
        }

        #region Standoff

        [Header("Standoff Parameters")]
        [SerializeField] private float _timeBeforeStage = 2.5f;
        [SerializeField] private float _fadeInDuration = 1f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _cycleDuration = 0.25f;
        private bool _standoffWinnerDeclared;
        private float _battleStartTime;
        private bool _haikuLineSelected;
        private List<LinePair> _haikuLineOptions;
        private int _currentLineChoice;
        private bool _haikuLastLine; // TODO - Remember to reset

        [Header("Events")] 
        [SerializeField] private GameEvent _onStandoffLineChanged;
        [SerializeField] private GameEvent _onPlayerInputSet;
        
        [FormerlySerializedAs("_haikuState")]
        [FormerlySerializedAs("_haikuLine")]
        [Header("Haiku Line Object")]
        [SerializeField] private HaikuStage _haikuStage;

        public void StartStandoff() => StartCoroutine(HaikuCountdown(_haikus));

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
            yield return new WaitForSeconds(_gameModeStartupBuffer);

            // Choose haiku
            Haiku haiku = _haikus[Random.Range(0, haikus.Count)];

            
            // Line 1
            yield return ExecuteStage(haiku.one);
            
            // Line 2
            yield return ExecuteStage(haiku.two);
            
            // Line 3
            yield return ExecuteStage(haiku.three); // TODO - Add ++ when battle start is included
            
            // // Battle Start
             //HaikuText.Instance.SetTexts(new LinePair("Strike!", "攻撃！"));
            //yield return awaitBattle;
            // EventManager.Events.StageX(stage);
            // _battleStartTime = Time.time;
            // yield return StartCoroutine(
              //   HaikuText.Instance.FadeText(0.05f, AnimationDirection.In));
        }

        private IEnumerator ExecuteStage(List<LinePair> lineOptions)
        {
            _haikuLineOptions = lineOptions;
            _currentLineChoice = 0;

            // Initial start up buffer for stage
            yield return new WaitForSeconds(_timeBeforeStage);
            
            // Set first option as current haiku text
            HaikuText.Instance.SetTexts(lineOptions[_currentLineChoice]);
            
            // Broadcast to all observers that this stage is starting
            _haikuStage.state++; // Increase HaikuStage's StandoffState
            _onStandoffLineChanged.TriggerEvent();
            
            // Fade in first Haiku Text
            yield return HaikuText.Instance.FadeText(_fadeInDuration, AnimationDirection.In);
            
            // While we have the HaikuStage reference, update it
            // ahead of time, after the CPU gets set, if it applies
            if (_haikuStage.state == StandoffState.Ready_CPU)
            {
                _haikuLastLine = true;
                _haikuStage.state = StandoffState.Ready_Player;
            }

            // Await until user selects haiku
            _haikuLineSelected = false;
            yield return new WaitUntil(() => _haikuLineSelected);
            
            // Fade out HaikuText
            yield return HaikuText.Instance.FadeText(_fadeOutDuration, AnimationDirection.Out);
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
        public void OnHaikuScroll(InputAction.CallbackContext obj)
        {
            // +1 for right, -1 for left
            _currentLineChoice = (int)obj.action.ReadValue<Vector2>().x == 1
                // Next stage
                ? (_currentLineChoice + 1) % _haikuLineOptions.Count
                // Previous stage
                : Math.Abs(_currentLineChoice - 1 + _haikuLineOptions.Count) 
                  % _haikuLineOptions.Count;

            StartCoroutine(CycleHaikuLine());

            IEnumerator CycleHaikuLine()
            {
                PlayerInputFlags haikuBuilderControls =
                    PlayerInputFlags.Scroll | PlayerInputFlags.Select;
                
                DisablePlayerControls(PlayerNumber.One, haikuBuilderControls);
                yield return HaikuText.Instance.FadeText(_cycleDuration, AnimationDirection.Out);
                HaikuText.Instance.SetTexts(_haikuLineOptions[_currentLineChoice]);
                yield return HaikuText.Instance.FadeText(_cycleDuration, AnimationDirection.In);
                EnablePlayerControls(PlayerNumber.One, haikuBuilderControls);
                
            }
        }

        /// Called when the Submit action is performed on both button-up and button-down.
        /// Awaits the button-up for when the Player needs to release the button in order to
        /// attack.
        public void OnHaikuSelect(InputAction.CallbackContext obj)
        {
            _haikuLineSelected = true;

            if (!_haikuLastLine) return;

            
            _onPlayerInputSet.TriggerEvent();
        }

        #endregion
    }
}
