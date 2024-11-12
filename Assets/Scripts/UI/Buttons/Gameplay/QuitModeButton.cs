using UnityEngine.EventSystems;

namespace UI.Buttons.Gameplay
{
    public class QuitModeButton : GameplayButton
    {
        public override void OnSubmit(BaseEventData eventData)
        {
            print("Quit");
        }
    }
}