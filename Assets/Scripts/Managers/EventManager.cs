using System;
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

        /// Fires when any MenuButton cancels out of its state
        public event Action<GameObject> OnMenuButtonCancel;

        public void MenuButtonCancel(GameObject subMenu)
        {
            OnMenuButtonCancel?.Invoke(subMenu);
        }

        /// Fires when any DefaultSubMenuButton cancels out of its state
        public event Action<GameObject> OnSubMenuButtonCancel;

        public void SubMenuButtonCancel(GameObject nowActiveButton)
        {
            OnSubMenuButtonCancel?.Invoke(nowActiveButton);
        }

        /// Fires when any DefaultSubMenuButton gets submitted
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
        
        #region Characters

        /// Fires when the selected GameMode should start
        public event Action OnRestartCurrentGameMode;

        public void RestartCurrentGameMode()
        {
            OnRestartCurrentGameMode?.Invoke();
        }

        #endregion
    }
}