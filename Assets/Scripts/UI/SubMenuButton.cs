using Managers;
using UnityEngine.EventSystems;

namespace UI
{
    public class SubMenuButton : Button  
    {
        public override void OnSubmit(BaseEventData eventData)
        {
            print("submitted this button");
        }

        public override void OnCancel(BaseEventData eventData)
        {
            print("cancelled this button");
            if (eventData.selectedObject == null)
            {
                EventManager.Events.MenuButtonCancel(null);
                return;
            }
            print(eventData.selectedObject);
        }

    }
}