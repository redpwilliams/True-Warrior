using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons.Menu
{
    public class OptionsButton : MainMenuButton
    {
        [SerializeField] private List<CharacterButton> _characterChoices;
        private OptionsButtonGroup _optionsButtonGroup;

        protected override void OnEnable()
        {
            base.OnEnable();
            _optionsButtonGroup =
                _buttonGroupGameObject.GetComponent<OptionsButtonGroup>();
        }

        // Sets the Options Menu first button to what is saved
        private void Start()
        {
            SamuraiType savedSamuraiType = SaveManager.LoadPlayerCharacter();
            foreach (var cb in _characterChoices.Where(
                         cb => cb.SamuraiType == savedSamuraiType))
            {
                SetFirstButton(cb.gameObject);
            }
        }
        
        /// Fires when the EventSystem/InputAction captures a
        /// submit input. This method tells its ButtonGroup
        /// to open the Sub Menu (ButtonGroup) associated with it
        public override void OnSubmit(BaseEventData eventData)
        {
            _manager.ShowButtonGroup(_optionsButtonGroup); // TODO
        }

    }
}