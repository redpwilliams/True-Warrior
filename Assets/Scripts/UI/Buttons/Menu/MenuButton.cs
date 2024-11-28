using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons.Menu
{
    public class MenuButton : BaseUIButton, IMoveHandler
    {
        [Header("Sub menu reference")] 
        [SerializeField] private GameObject _subMenu;
        [SerializeField] private GameObject _firstButton;
        [SerializeField] private SubMenuButtonGroup _subMenuButtonGroup;

        [Header("MainMenuManager Reference")]
        [SerializeField] private MainMenuButtonGroup _manager;

        /// Fires when the EventSystem/InputAction captures a
        /// submit input. This method tells its ButtonGroup
        /// to open the Sub Menu (ButtonGroup) associated with it
        public override void OnSubmit(BaseEventData eventData)
        {
            _manager.ShowButtonGroup(_subMenuButtonGroup); // TODO
        }

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
                    OnSubmit(eventData);
                    break;

                case MoveDirection.Up:
                case MoveDirection.Down:
                case MoveDirection.None:
                default: return;
            }
        }

        public void SetFirstButton(GameObject newFirstButton)
        {
            this._firstButton = newFirstButton;
        }
    }
}