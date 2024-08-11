using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SubMenuButton : Button
    {
        [SerializeField] private GameObject _parentButton;
        [SerializeField] private GameMode _gameMode;

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            StartCoroutine(WaitAndCheck(CheckAsync));
            
            IEnumerator CheckAsync()
            {
                yield return MoveButton(false);
                if (EventSystem.current.currentSelectedGameObject is null)
                    EventManager.Events.SubMenuButtonCancel(null); // Button was clicked off
            }
        }
        
        public override void OnSubmit(BaseEventData eventData)
        {
            // Turn off interactivity for all buttons
            EventManager.Events.SubMenuButtonSubmit(_gameMode); 
        }

        public override void OnCancel(BaseEventData eventData)
        {
            StartCoroutine(WaitAndCheck(CheckAsync));

            IEnumerator CheckAsync()
            {
                yield return MoveButton(false);
                EventManager.Events.SubMenuButtonCancel(
                    EventSystem.current.currentSelectedGameObject is null 
                        ? null 
                        : _parentButton);
            }
        }

        public override void OnPointerClick(PointerEventData pointerEventData)
            => OnSubmit(pointerEventData);
        /*
         * TODO - If this button is deselected then gets clicked on,
         * it should set the button as selected instead of submitting it.
         * If this button is selected then gets clicked on,
         * it should submit it.
         */
    }
}