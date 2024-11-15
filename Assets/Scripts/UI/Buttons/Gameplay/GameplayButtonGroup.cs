using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace UI.Buttons.Gameplay
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameplayButtonGroup : MonoBehaviour
    {
        private CanvasGroup _cg;
        [SerializeField] private GameObject _firstButton;
        [SerializeField] private List<GameplayButton> _buttons;

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
                yield return AnimateButtonGroup(AnimationDirection.In);
                EventSystem.current.SetSelectedGameObject(_firstButton);
            }
        }

        public void HideButtons()
        {
            StartCoroutine(ButtonStateChange());

            IEnumerator ButtonStateChange()
            {
                yield return AnimateButtonGroup(AnimationDirection.Out);
                gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
                
                // Deselect all buttons (visually from UI)
                foreach (var button in _buttons)
                {
                    button.SetButton(BaseUIButton.ButtonState.InActive);
                }
            }
        }

        private IEnumerator AnimateButtonGroup(AnimationDirection direction)
        {
            float startAlpha = _cg.alpha;
            float endAlpha = (direction == AnimationDirection.In) ? 1 : 0;

            float elapsedTime = 0;
            float animationDuration = 0.5f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                _cg.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _cg.alpha = endAlpha;
        }

    }
}