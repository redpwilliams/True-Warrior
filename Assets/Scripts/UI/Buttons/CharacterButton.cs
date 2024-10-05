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
            
            // StartCoroutine(WaitAndCheck(CheckAsync));
            //
            // IEnumerator CheckAsync()
            // {
            //     yield return MoveButton(false);
            //     EventManager.Events.SubMenuButtonCancel(
            //         EventSystem.current.currentSelectedGameObject is null 
            //             ? null 
            //             : _parentButton);
            // }
        }
    }
}