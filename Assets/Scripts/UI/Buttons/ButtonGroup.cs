using System.Collections.Generic;
using UnityEngine;

namespace UI.Buttons
{
    /// A GameObject that contains a collection of Buttons
    public class ButtonGroup : MonoBehaviour
    {
        /// The buttons associated with this ButtonGroup
        [SerializeField] protected List<BaseUIButton> _buttons;
    }
}