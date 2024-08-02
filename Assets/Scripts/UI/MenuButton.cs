using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MenuButton : MonoBehaviour, ISelectHandler, 
    IDeselectHandler, ISubmitHandler
    {
        private readonly float _moveOffset = 15f;
        [SerializeField] private GameObject _enChild;
        [SerializeField] private GameObject _jpChild;
        private RectTransform _en, _jp;

        public enum ButtonClass
        {
            Play,
            Instructions,
            Options,
            Quit
        };

        [SerializeField] private ButtonClass _type;

        private void Start()
        {
            _en = _enChild.GetComponent<RectTransform>();
            _jp = _jpChild.GetComponent<RectTransform>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            StartCoroutine(Move(true));
        }

        public void OnDeselect(BaseEventData eventData)
        {
            StartCoroutine(Move(false));
        }

        public void OnSubmit(BaseEventData eventData)
        {
            EventManager.Events.MenuButtonSubmit(_type);
        }

        private IEnumerator Move(bool outWards)
        {
            // print(this.name + " is moving " + (outWards ? "outwards" : "inwards"));
            float start = outWards ? 0 : _moveOffset;
            float end = outWards ? _moveOffset : 0;
            
            var enLocalPosition = _en.localPosition;
            var jpLocalPosition = _jp.localPosition;
            Vector3 enStartPosition = new Vector3(start, enLocalPosition.y, enLocalPosition.z);
            Vector3 jpStartPosition = new Vector3(start, jpLocalPosition.y, jpLocalPosition.z);
            Vector3 enEndPosition = new Vector3(end, enLocalPosition.y, enLocalPosition.z);
            Vector3 jpEndPosition = new Vector3(end, jpLocalPosition.y, jpLocalPosition.z);

            float timeElapsed = 0f;
            float moveDuration = 0.1f;

            while (timeElapsed < moveDuration)
            {
                float t = timeElapsed / moveDuration;

                _en.localPosition = Vector3.Lerp(enStartPosition, enEndPosition, t);
                _jp.localPosition = Vector3.Lerp(jpStartPosition, jpEndPosition, t);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _en.localPosition = enEndPosition;
            _jp.localPosition = jpEndPosition;
        }
    }
}