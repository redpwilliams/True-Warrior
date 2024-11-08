using UnityEngine.EventSystems;

namespace UI.Buttons.Gameplay
{
    public abstract class GameplayButton : BaseUIButton
    {
        public override void OnCancel(BaseEventData eventData) =>
            base.OnDeselect(eventData); 
    }
}