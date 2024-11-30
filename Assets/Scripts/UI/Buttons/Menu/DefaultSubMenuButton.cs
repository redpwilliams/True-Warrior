using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using GameMode = Managers.GameManager.GameMode;

namespace UI.Buttons.Menu
{
    [System.Serializable]
    public class DefaultSubMenuButton : BaseUIButton, IMoveHandler
    {
        /// The Button game object to go to after cancelling out of this
        /// button/the play submenu altogether
        [SerializeField] private GameObject _parentButton;
        
        /// The GameMode of this button
        /// TODO: Discard after creating Standoff, Survival, and Zen button
        [SerializeField] private GameMode _gameMode;

        
        /// Deselects this button, going to the main menu if inputted. 
        public override void OnDeselect(BaseEventData eventData)
        {
            print("ran deselect");
            base.OnDeselect(eventData);
            StartCoroutine(WaitAndCheck(CheckAsync));
            
            // Waits to see if the button was clicked off
            // REVIEW: No longer necessary (temporarily) since pointer events
            // were turned off
            IEnumerator CheckAsync()
            {
                yield return MoveButton(ButtonState.InActive);
                if (EventSystem.current.currentSelectedGameObject is null)
                    EventManager.Events.SubMenuButtonCancel(null); // Button was clicked off
            }
        }
        
        /// Submits this button and begins its game mode
        /// REVIEW: Make this abstract. The StandoffButton, etc., should
        /// run a method to start the game mode. If I recall correctly, there
        /// two events happening. This should cut down on one.
        public override void OnSubmit(BaseEventData eventData)
        {
            // Turn off interactivity for all buttons
            EventManager.Events.DisableAllButtons(_gameMode);
            
            // TODO: Tell PlayerPrefs to update the default button
            
            StartCoroutine(TransitionUIToGame());

            IEnumerator TransitionUIToGame()
            {
                yield return UICanvas.Canvas.FadeCanvas();
                UICanvas.Canvas.SetUICanvasInactive();
                UICanvas.Canvas.SetGameCanvasActive();
                
                // Start Button's corresponding GameMode
                GameManager.Manager.StartGameMode(_gameMode);
            }
        }
        
        
        public override void OnCancel(BaseEventData eventData)
        {
            StartCoroutine(WaitAndCheck(CheckAsync));
        
            IEnumerator CheckAsync()
            {
                yield return MoveButton(ButtonState.InActive);
                EventManager.Events.SubMenuButtonCancel(
                    EventSystem.current.currentSelectedGameObject is null 
                        ? null 
                        : _parentButton);
            }
        }

        /// Handles navigation input.
        /// This button only needs to know how to cancel out of itself
        /// and start the next game
        public void OnMove(AxisEventData eventData)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    OnCancel(eventData);
                    break;
                
                case MoveDirection.Right:
                    OnSubmit(eventData);
                    break;
                
                case MoveDirection.Up:
                case MoveDirection.Down:
                case MoveDirection.None:
                default: return;
            }
        }
    }
}