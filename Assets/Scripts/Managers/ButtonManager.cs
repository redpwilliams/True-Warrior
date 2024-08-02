using System.Collections;
using Characters;
using UI;
using UnityEngine;

namespace Managers
{
    public class ButtonManager : MonoBehaviour
    {

        private float _moveOffset = -200f;
        private RectTransform _rt;

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
            EventManager.Events.OnMenuButtonSubmit += HandleSubmit;
        }

        private void HandleSubmit(MenuButton.ButtonClass type)
        {
            print(type);
            StartCoroutine(MoveMenu(true));
        }

        private IEnumerator MoveMenu(bool isSubmit)
        {
            float start = isSubmit ?  0 : _moveOffset;
            float end = isSubmit  ? _moveOffset : 0;

            var localPosition = _rt.localPosition;

            Vector3 startPosition = new Vector3(start, localPosition.y, localPosition.z);
            Vector3 endPosition = new Vector3(end, localPosition.y, localPosition.z);

            float timeElapsed = 0;
            float moveDuration = 0.1f;

            while (timeElapsed < moveDuration)
            {
                float t = timeElapsed / moveDuration;
                _rt.localPosition = Vector3.Lerp(startPosition, endPosition,
                    Lerp2D.EaseOutQuart(t));

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _rt.localPosition = endPosition;
        }
    }
}