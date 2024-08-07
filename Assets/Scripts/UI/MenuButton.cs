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
            StartCoroutine(WaitAndCheck(() 
                => EventManager.Events.MenuButtonCancel(_subMenu)));
        }

        public override void OnSubmit(BaseEventData eventData) 
            => EventManager.Events.MenuButtonSubmit(_subMenu, _firstButton);

        public override void OnCancel(BaseEventData eventData) 
            => EventManager.Events.MenuButtonCancel(_subMenu);

    }
}