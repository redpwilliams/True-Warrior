using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SubMenuButton : Button
    {
        [SerializeField] private GameObject _parentButton;

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
            print("submitted this button");
            // Turn off interactivity for all buttons
            EventManager.Events.SubMenuButtonSubmit(); // TODO - Re-enable at some point
            // Start ui canvas fade out transition
            // Start selected game mode
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