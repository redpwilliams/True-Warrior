using System.Collections;
using UnityEngine;
using TMPro;

namespace Characters
{
    public class CharacterText : MonoBehaviour
    {
        private TextMeshPro _tmp;
        private RectTransform _rt;

        private readonly float _initialRiseDelay = 0.6f;
        private readonly float _riseDuration = 0.35f;
        private readonly float _stayDuration = 1.75f;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshPro>();
            _rt = GetComponent<RectTransform>();
            // _tmp.enabled = false; // Use setActive
        }

        /// Sets text of TMP asset
        public void SetText(string text)
        {
            _tmp.enabled = true;
            _tmp.text = text;
        }

        public void DisplayTitle(string title)
        {
            //_tmp.alpha = 0;
            _tmp.enabled = true;
            _tmp.text = title;

            StartCoroutine(RiseAndDisplay());

        }
        
        private IEnumerator RiseAndDisplay()
        {
            // Wait before rising
            yield return new WaitForSeconds(_initialRiseDelay);
            
            float elapsedTime = 0f;

            Vector2 currentPosition = _rt.anchoredPosition;
            float riseDistance = 0.3f;
            Vector2 targetPosition =
                currentPosition + Vector2.up * riseDistance;
                
            while (elapsedTime < _riseDuration)
            {
                float t = elapsedTime / _riseDuration;
                // Set y position of text
                _rt.anchoredPosition = Vector2.Lerp(currentPosition, 
                targetPosition, Lerp2D.EaseOutQuad(t));
                    
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _rt.anchoredPosition = targetPosition;
        }

    }
}