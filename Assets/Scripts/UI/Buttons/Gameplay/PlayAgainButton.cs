using UnityEngine.EventSystems;

namespace UI.Buttons.Gameplay
{
    public class PlayAgainButton : InGameButton
    {
        public override void OnSubmit(BaseEventData eventData)
        {
            print("Play again");
        }
    }
}