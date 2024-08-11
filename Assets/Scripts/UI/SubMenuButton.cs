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
    }
}