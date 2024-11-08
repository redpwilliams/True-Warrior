using UnityEngine.EventSystems;

namespace UI.Buttons.Gameplay
{
    public class PlayAgainButton : InGameButton
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            print("Play again");
        }
    }
}