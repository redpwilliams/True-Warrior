using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
    /// A button that represents a character the player can choose to play as
    public class CharacterButton : MonoBehaviour, ISelectHandler, 
    IDeselectHandler, ICancelHandler, ISubmitHandler
    {
        [SerializeField] private GameObject _parentButton;
        [SerializeField] private TextMeshPro _chosenTextObject;
        private CharacterSelectSprite _css;
        [SerializeField] private SamuraiType _samuraiType;
        private static MenuButton _optionsMenuButton;

        private void OnEnable()
        {
            _css = GetComponent<CharacterSelectSprite>();
            _optionsMenuButton = _parentButton.GetComponent<MenuButton>();

            EventManager.Events.OnDeselectAllChosenTOs +=
                HideChosenTextObject;
            
            if (SaveManager.LoadPlayerCharacter() == _samuraiType)
                ShowChosenTextObject();
                
        }

        public void OnSelect(BaseEventData eventData)
        {
            print(gameObject.name + " is selected");
            _css.StartAnimation();
            _css.BrightenSpriteLight();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            print(gameObject.name + " is deselected");
            _css.StopAnimation();
            _css.DimSpriteLight();
            
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

        public void OnSubmit(BaseEventData eventData)
        {
            // Do nothing if this one is already selected
            if (_chosenTextObject.IsActive()) return;
            
            // Since it isn't selected, deselect the all/last one
            EventManager.Events.DeselectAllChosenTOs();
                
            // Reselect this one
            _chosenTextObject.gameObject.SetActive(true);
            
            // Save selection
            SaveManager.SavePlayerCharacter(_samuraiType);
            
            // Update what Options button points to when selected
            _optionsMenuButton.SetFirstButton(this.gameObject);
        }

        private void ShowChosenTextObject() 
            => _chosenTextObject.gameObject.SetActive(true);

        private void HideChosenTextObject() 
            => _chosenTextObject.gameObject.SetActive(false);
    }
    
}