using UnityEngine.EventSystems;

namespace UI.Buttons.Gameplay
{
    public class ReturnButton : GameplayButton
    {
        public override void OnSubmit(BaseEventData eventData)
        {
            print("Quit");
        }
    }
}