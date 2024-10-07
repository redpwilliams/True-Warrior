using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

namespace UI.Buttons
{
    public class OptionsMenuButton : MenuButton
    {
        [SerializeField] private List<CharacterButton> _characterChoices;

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
    }
}