using System.Collections;
using Characters;
using UnityEngine;

namespace Managers
{
    public class ButtonManager : MonoBehaviour
    {

        private readonly float _moveOffset = -200f;
        private RectTransform _rt;
        private bool _buttonIsSelected;

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
            EventManager.Events.OnMenuButtonSubmit += HandleSubmit;
            EventManager.Events.OnMenuButtonCancel += HandleCancel;
        }

        private void HandleSubmit(GameObject subMenu)
        {
            if (_buttonIsSelected) return;
            _buttonIsSelected = true;
            StartCoroutine(MoveMenu(true, subMenu));
        }

        private void HandleCancel(GameObject subMenu)
        {
            if (!_buttonIsSelected) return;
            StartCoroutine(MoveMenu(false, subMenu));
            _buttonIsSelected = false;
        }

        private IEnumerator MoveMenu(bool isSubmit, GameObject subMenu)
        {
            // Remove sub menu before animating main menu move in
            if (!isSubmit) subMenu.SetActive(false);
            
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
            
            // Show sub menu after animating main menu move out
            if (isSubmit) subMenu.SetActive(true);
        }
    }
}