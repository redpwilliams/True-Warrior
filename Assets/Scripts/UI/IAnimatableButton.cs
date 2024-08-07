using System.Collections;
using UnityEngine;

namespace UI
{
    public interface IAnimatableButton
    {
        public IEnumerator MoveButton(bool outWards);
    }
}