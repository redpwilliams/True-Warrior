using UnityEngine.EventSystems;

namespace UI.Buttons.Gameplay
{
    public abstract class InGameButton : BaseUIButton
    {
        public override void OnCancel(BaseEventData eventData) =>
            base.OnDeselect(eventData); 
    }
}