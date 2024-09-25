using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
    public class MenuButton : Button
    {
        [Header("Sub menu reference")] 
        [SerializeField] private GameObject _subMenu;
        [SerializeField] private GameObject _firstButton;

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            StartCoroutine(WaitAndCheck(() =>
            {
                if (EventSystem.current.currentSelectedGameObject is null)
                    EventManager.Events.MenuButtonCancel(_subMenu);
            }));
        }
        
        public override void OnSubmit(BaseEventData eventData) 
            => EventManager.Events.MenuButtonSubmit(_subMenu, _firstButton);

        public override void OnCancel(BaseEventData eventData) 
            => EventManager.Events.MenuButtonCancel(_subMenu);

        public override void OnPointerClick(PointerEventData pointerEventData)
        {
            if (_subMenu.activeInHierarchy)
            {
                EventManager.Events.SubMenuButtonCancel(this.gameObject);
            }
            else
            {
                OnSubmit(pointerEventData);
            }
        }
        // TODO: Do nothing?

    }
}