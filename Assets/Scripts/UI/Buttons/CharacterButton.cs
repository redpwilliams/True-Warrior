using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
    /// A button that represents a character the player can choose to play as
    public class CharacterButton : MonoBehaviour, ISelectHandler, 
    IDeselectHandler, ICancelHandler
    {
        [SerializeField] private GameObject _parentButton;
        private CharacterSelectSprite _css;

        private void OnEnable()
        {
            _css = GetComponent<CharacterSelectSprite>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            print(gameObject.name + " is selected");
            _css.StartAnimation();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            print(gameObject.name + " is deselected");
            _css.StopAnimation();
            
            // Trigger the cancel event when the player
            // exits the options menu via the left most
            // character select button
            StartCoroutine(HandleCancelByDeselect());
            
            IEnumerator HandleCancelByDeselect()
            {
                // Wait for one frame
                yield return null;
                
                // The options sub menu should be canceled if
                // a deselect (on the leftmost CharacterSelectSprite)
                // set it to the options menu
                if (EventSystem.current.currentSelectedGameObject.Equals(
                        _parentButton))
                {
                    EventManager.Events.SubMenuButtonCancel(_parentButton);
                }
            }
        }


        // InputAction map programs this to escape key
        public void OnCancel(BaseEventData eventData)
        {
            print("Ran cancel");
            
            // TODO: Use coroutine to fade out menu
            EventManager.Events.SubMenuButtonCancel(
                EventSystem.current.currentSelectedGameObject is null 
                    ? null 
                    : _parentButton);
            
        }
    }
}