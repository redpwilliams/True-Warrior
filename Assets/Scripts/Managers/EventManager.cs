using System;
using Characters;
using UnityEngine;
using GameMode = Managers.GameManager.GameMode;

namespace Managers
{
    /// <summary>Handles all events in the game</summary>
    /// <remarks>
    ///     EventManager provides implementation for the subscription and dispatching of
    ///     all events in the game. <para />
    ///     To subscribe to an event,
    ///     <code>
    ///         EventManager.events.EventAction += MyMethod
    ///     </code>
    ///     <para />
    ///     To dispatch an event,
    ///     <code>
    ///         EventManager.events.OnYourEvent()
    ///     </code>
    /// </remarks>
    public sealed class EventManager : MonoBehaviour
    {
        /// Singleton Instance
        public static EventManager Events { get; private set; }

        private void Awake()
        {
            if (Events != null && Events != this)
            {
                Destroy(Events);
                return;
            }

            Events = this;
            DontDestroyOnLoad(gameObject);
        }

        public event Action OnBeginAttack;

        public void BeginAttack()
        {
            OnBeginAttack?.Invoke();
        }

        #region Staging Logic

        private double _battleStartTime;
        public event Action<int> OnStageX;

        public void StageX(int stage)
        {
            if (stage == 3) _battleStartTime = Time.realtimeSinceStartupAsDouble;
            OnStageX?.Invoke(stage);
        }

        #endregion

        #region Menu

        /// Fires when any MenuButton is logged as "submitted"
        public event Action<GameObject, GameObject> OnMenuButtonSubmit;

        public void MenuButtonSubmit(GameObject subMenu, GameObject nowActiveButton)
        {
            OnMenuButtonSubmit?.Invoke(subMenu, nowActiveButton);
        }

        /// Fires when any MenuButton cancels out of its state
        public event Action<GameObject> OnMenuButtonCancel;

        public void MenuButtonCancel(GameObject subMenu)
        {
            OnMenuButtonCancel?.Invoke(subMenu);
        }

        /// Fires when any SubMenuButton cancels out of its state
        public event Action<GameObject> OnSubMenuButtonCancel;

        public void SubMenuButtonCancel(GameObject nowActiveButton)
        {
            OnSubMenuButtonCancel?.Invoke(nowActiveButton);
        }

        /// Fires when any SubMenuButton gets submitted
        public event Action<GameMode> OnDisableAllButtons;

        public void DisableAllButtons(GameMode gm)
        {
            OnDisableAllButtons?.Invoke(gm);
        }

        /// Fires when a new SamuraiClass is chosen by submitting
        /// a CharacterButton
        public event Action OnDeselectAllChosenTOs;

        public void DeselectAllChosenTOs()
        {
            OnDeselectAllChosenTOs?.Invoke();
        }
        
        #endregion

        #region Game Start

        /// Fires when the selected GameMode should start
        public event Action<GameMode> OnGameModeStart;

        public void GameModeStart(GameMode gm)
        {
            OnGameModeStart?.Invoke(gm);
        }

        #endregion

        #region Input

        // Relays who hit first?
        // Log winner attack timestamp. If it's less than X seconds later, any incoming (?) inputs are marked as late
        private Character _winner;
        private bool _winnerDeclared;

        public ReactionInfo CharacterInputsAttack(Character c, double time)
        {
            double reactionTime = time - _battleStartTime;
            string formattedReactionTime = $"{reactionTime:F3}";

            if (!_winnerDeclared)
            {
                _winnerDeclared = true;
                _winner = c;
            }

            return new ReactionInfo(_winner, formattedReactionTime);
        }

        #endregion
    }
}