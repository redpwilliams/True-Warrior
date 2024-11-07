using UnityEngine.EventSystems;

namespace UI.Buttons
{
    public abstract class InGameButton : BaseUIButton
    {
        public override void OnCancel(BaseEventData eventData) =>
            base.OnDeselect(eventData); 
    }
}