using UnityEngine.EventSystems;

namespace UI
{
    public class SubMenuButton : Button  
    {

        public override void OnSelect(BaseEventData eventData) => StartCoroutine
        (MoveButton(true));
        
        public void OnDeselect(BaseEventData eventData) => StartCoroutine(MoveButton(false));


        public void OnSubmit(BaseEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnCancel(BaseEventData eventData)
        {
            throw new System.NotImplementedException();
        }

    }
}