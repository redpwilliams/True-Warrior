using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons.Menu
{
    /// The base class for the main menu buttons like Play, Options, 
    /// Instructions, and Quit. Leaves room for further inheritance.
    public class DefaultMenuButton : BaseUIButton, IMoveHandler
    {
        [SerializeField] private GameObject _firstButton;
        
        [Tooltip("This GameObject is the ButtonGroup this DefaultMenuButton will open.")]
        [SerializeField] protected GameObject _buttonGroupGameObject;
        
        /// The _buttonGroupGameObject's DefaultSubMenu component
        private DefaultSubMenu _subMenu;

        [Tooltip("The Main Menu reference.")]
        [SerializeField] protected MainMenu _mainMenu;

        /// Calls BaseUIButton.OnEnable and stores a reference to this
        /// button's corresponding Sub Menu.
        protected override void OnEnable()
        {
            base.OnEnable();
            _subMenu = _buttonGroupGameObject.GetComponent<DefaultSubMenu>();
        }
        
        /// Fires when the EventSystem/InputAction captures a
        /// submit input. This method tells its ButtonGroup
        /// to open the Sub Menu (ButtonGroup) associated with it
        public override void OnSubmit(BaseEventData eventData)
        {
            _mainMenu.ShowButtonGroup(_subMenu);
        }

        public override void OnCancel(BaseEventData eventData) 
            => EventManager.Events.MenuButtonCancel(_buttonGroupGameObject);


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