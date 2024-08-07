using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MenuButton : Button
    {
        [Header("Sub menu reference")] 
        [SerializeField] private GameObject _subMenu;
        [SerializeField] private GameObject _firstButton;

        public override void OnDeselect(BaseEventData eventData)
        {
            StartCoroutine(MoveButton(false));
            StartCoroutine(WaitAndCheck());
        }
        
        protected override IEnumerator WaitAndCheck()
        {
            // Wait one frame
            // (to let EventSystem update currentSelectedGameObject
            yield return null;

            if (EventSystem.current.currentSelectedGameObject is null)
                EventManager.Events.MenuButtonCancel(_subMenu);
        }


        public override void OnSubmit(BaseEventData eventData) 
            => EventManager.Events.MenuButtonSubmit(_subMenu, _firstButton);

        public override void OnCancel(BaseEventData eventData) 
            => EventManager.Events.MenuButtonCancel(_subMenu);
        // TODO: Do nothing?

    }
}