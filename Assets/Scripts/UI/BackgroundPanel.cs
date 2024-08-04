using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class BackgroundPanel : MonoBehaviour, IPointerDownHandler
    {

        public void OnPointerDown(PointerEventData eventData)
        {
            print(EventSystem.current.IsPointerOverGameObject());
            print(eventData.selectedObject);
        }
    }
}