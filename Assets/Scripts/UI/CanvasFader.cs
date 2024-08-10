using System.Collections;
using Managers;
using Characters;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFader : MonoBehaviour
    {
        private CanvasGroup _cg;
        private void OnEnable()
        {
            _cg = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            EventManager.Events.OnSubMenuButtonSubmit += FadeCanvas;
        }

        private void OnDisable()
        {
            EventManager.Events.OnSubMenuButtonSubmit -= FadeCanvas;
        }

        private void FadeCanvas()
        {
            StartCoroutine(Fade());

            IEnumerator Fade()
            {
                float animationDuration = 1.5f;
                float elapsedTime = 0;

                while (elapsedTime < animationDuration)
                {
                    float t = elapsedTime / animationDuration;
                    _cg.alpha = Mathf.Lerp(1, 0, Lerp2D.EaseInCubic(t));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _cg.alpha = 0;
                this.gameObject.SetActive(false);
            }
        }

    }
}