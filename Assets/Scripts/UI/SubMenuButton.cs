using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SubMenuButton : MonoBehaviour, ISelectHandler, 
    IDeselectHandler, ISubmitHandler, ICancelHandler
    {
        public void OnSelect(BaseEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            throw new System.NotImplementedException();
        }

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