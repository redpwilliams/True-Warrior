using UnityEngine.EventSystems;

namespace UI.Buttons.Gameplay
{
    public class PlayAgainButton : GameplayButton
    {
        public override void OnSubmit(BaseEventData eventData)
        {
            print("Play again");
        }
    }
}