using System.Collections.Generic;
using UI.Buttons.Menu;
using UnityEngine;

namespace UI.Buttons
{
    /// A GameObject that contains a collection of Buttons
    public abstract class ButtonGroup<T> : MonoBehaviour where T : BaseUIButton
    {
        /// The buttons associated with this ButtonGroup
        [SerializeField] protected List<T> _buttons;

        public BaseUIButton DefaultButton => _buttons[0];

        protected virtual void Start()
        {
            ManagerHandshake();
        }

        protected virtual void ManagerHandshake()
        {
            // Handshake
            foreach (var button in _buttons)
            {
                button.Manager = this as ButtonGroup<BaseUIButton>;
            }
        }


        public abstract void ShowButtonGroup<TB>(ButtonGroup<TB> bg)
            where TB : BaseUIButton;
    }
}