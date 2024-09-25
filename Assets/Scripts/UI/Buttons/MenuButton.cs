using System;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
    public class MenuButton : Button, IMoveHandler
    {
        [Header("Sub menu reference")] 
        [SerializeField] private GameObject _subMenu;
        [SerializeField] private GameObject _firstButton;

        private bool _isCurrentlySelected;

        // public override void OnDeselect(BaseEventData eventData)
        // {
        //     base.OnDeselect(eventData);
        //     // StartCoroutine(WaitAndCheck(() =>
        //     // {
        //     //     if (EventSystem.current.currentSelectedGameObject is null)
        //     //         EventManager.Events.MenuButtonCancel(_subMenu);
        //     // }));
        // }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _isCurrentlySelected = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            _isCurrentlySelected = false;
        }
        
        public override void OnSubmit(BaseEventData eventData)
        => EventManager.Events.MenuButtonSubmit(_subMenu, _firstButton);

        public override void OnCancel(BaseEventData eventData) 
            => EventManager.Events.MenuButtonCancel(_subMenu);


        // public override void OnPointerClick(PointerEventData pointerEventData)
        // {
        //     if (_subMenu.activeInHierarchy)
        //     {
        //         EventManager.Events.SubMenuButtonCancel(this.gameObject);
        //     }
        //     else
        //     {
        //         OnSubmit(pointerEventData);
        //     }
        // }
        // TODO: Do nothing?

        public void OnMove(AxisEventData eventData)
        {
            // Handle Left/Right inputs
            switch (eventData.moveDir)
            { 
                /* REVIEW:
                 This should only be possible if the main menu
                 is the only one active. It should just de-select
                 this button.
                 */ 
                case MoveDirection.Left:
                    print("Ran deselect");
                    OnDeselect(eventData);
                    
                    // stops button "appearing" deselected to match expected state
                    EventSystem.current.SetSelectedGameObject(null);
                    break;
                
                /* REVIEW:
                 This should open a sub menu. Needs to be refactored later
                 because the quit button has no sub menu.
                 */
                case MoveDirection.Right:
                    _isCurrentlySelected = false;
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