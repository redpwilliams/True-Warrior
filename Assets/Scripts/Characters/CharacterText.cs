using System.Collections;
using UnityEngine;
using TMPro;

namespace Characters
{
    public class CharacterText : MonoBehaviour
    {
        private TextMeshPro _tmp;
        private RectTransform _rt;

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

        public void DisplayTitle(string title, float aD, float sD)
        {
            //_tmp.alpha = 0;
            _tmp.enabled = true;
            _tmp.text = title;

            StartCoroutine(RiseAndDisplay(aD, sD));

        }
        
        private IEnumerator RiseAndDisplay(float animationDuration, float 
        stayDuration)
        {
            float elapsedTime = 0f;

            Vector2 currentPosition = _rt.anchoredPosition;
            float riseDistance = 0.3f;
            Vector2 targetPosition =
                currentPosition + Vector2.up * riseDistance;
                
            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                // Set y position of text
                _rt.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, t);
                    
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _rt.anchoredPosition = targetPosition;
        }

    }
}