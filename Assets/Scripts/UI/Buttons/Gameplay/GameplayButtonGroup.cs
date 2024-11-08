using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons.Gameplay
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameplayButtonGroup : MonoBehaviour
    {
        private CanvasGroup _cg;
        [SerializeField] private GameObject _firstButton;

        private enum Animate
        {
            In,
            Out
        }
        
        private void Awake()
        {
            _cg = GetComponent<CanvasGroup>();
            _cg.alpha = 0;
        }

        public void ShowButtons()
        {
            gameObject.SetActive(true);
            StartCoroutine(ButtonStateChange());

            IEnumerator ButtonStateChange()
            {
                yield return AnimateButtonGroup(Animate.In);
                EventSystem.current.SetSelectedGameObject(_firstButton);
            }
        }

        public void HideButtons()
        {
            StartCoroutine(ButtonStateChange());

            IEnumerator ButtonStateChange()
            {
                yield return AnimateButtonGroup(Animate.Out);
                gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private IEnumerator AnimateButtonGroup(Animate direction)
        {
            float startAlpha = _cg.alpha;
            float endAlpha = (direction == Animate.In) ? 1 : 0;

            float elapsedTime = 0;
            float animationDuration = 0.5f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                _cg.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

    }
}