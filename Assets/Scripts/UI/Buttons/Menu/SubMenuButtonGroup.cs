using System;
using System.Collections.Generic;

namespace UI.Buttons.Menu
{
    public class SubMenuButtonGroup : ButtonGroup<BaseUIButton>
    {
        public List<BaseUIButton> Buttons;

        private void Awake()
        {
            foreach (var button in _buttons)
            {
                Buttons.Add(button);
            }
        }
    }
}