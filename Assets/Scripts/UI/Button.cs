using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public abstract class Button : MonoBehaviour, ISelectHandler, 
        IDeselectHandler, ISubmitHandler, ICancelHandler
    {
        private readonly float _moveOffset = 15f;
        [Header("Child references")]
        [SerializeField] private GameObject _enChild;
        [SerializeField] private GameObject _jpChild;
        private RectTransform _en, _jp;

        private void OnEnable()
        {
            _en = _enChild.GetComponent<RectTransform>();
            _jp = _jpChild.GetComponent<RectTransform>();
        }
        protected IEnumerator MoveButton(bool outWards)
        {
            // Wait one frame - gives time for Start?
            
            
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

        public abstract void OnSelect(BaseEventData eventData);

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