using System.Collections.Generic;
using UnityEngine;

namespace UI.Buttons
{
    /// A GameObject that contains a collection of Buttons
    public class ButtonGroup<T> : MonoBehaviour where T : BaseUIButton
    {
        /// The buttons associated with this ButtonGroup
        [SerializeField] protected List<T> _buttons;
    }
}