using System.Collections;
using UnityEngine;
using TMPro;
using Util;

namespace Characters
{
    [RequireComponent(typeof(TextMeshPro))]
    [RequireComponent(typeof(RectTransform))]
    public class CharacterText : MonoBehaviour
    {
        private TextMeshPro _tmp;
        private RectTransform _rt;

        private readonly float _initialRiseDelay = 0.6f;
        private readonly float _riseDuration = 0.35f;
        private readonly float _stayDuration = 1.75f;
        private readonly float _fadeoutDuration = 0.5f;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshPro>();
            _rt = GetComponent<RectTransform>();
        }

        /// Sets text of TMP asset
        public void ShowReactionTime(string text)
        {
            _tmp.text = text;
            _tmp.alpha = 1;
        }

        public void DisplayTitle(string title)
        {
            _tmp.alpha = 0;
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
                float lerpTime = Lerp2D.EaseOutQuad(elapsedTime / _riseDuration);
                
                // Set y position of text
                _rt.anchoredPosition = Vector2.Lerp(currentPosition, 
                targetPosition, lerpTime);
                
                // Set alpha
                _tmp.alpha = Mathf.Lerp(0f, 1f, lerpTime);
                    
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Snap to target position
            _rt.anchoredPosition = targetPosition;
            
            // Initial wait to fade out text
            yield return new WaitForSeconds(_stayDuration);
            
            // Fade out text
            elapsedTime = 0f;

            while (elapsedTime < _fadeoutDuration)
            {
                float t = elapsedTime / _fadeoutDuration;
                _tmp.alpha = Mathf.Lerp(1, 0, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _tmp.alpha = 0;
        }

        public void SetDirection()
        {
            // SetDirection the text to match with a parent's change in direction
            _rt.localScale = new Vector3(Mathf.Sign(transform.parent
                .localScale.x), 1, 1);
        }

    }
}