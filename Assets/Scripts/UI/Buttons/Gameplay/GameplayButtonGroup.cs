using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace UI.Buttons.Gameplay
{
    /// A GameObject with multiple GameplayButtons
    [RequireComponent(typeof(CanvasGroup))]
    public class GameplayButtonGroup : ButtonGroup<GameplayButton>
    {
        /// CanvasGroup component used for alpha animation
        private CanvasGroup _cg;
        
        private void Awake()
        {
            _cg = GetComponent<CanvasGroup>();
            _cg.alpha = 0;
        }

        /// Animates in the ButtonGroup
        public void ShowButtons()
        {
            // Sets the ButtonGroup to active, before animating
            gameObject.SetActive(true);
            StartCoroutine(ButtonStateChange());

            IEnumerator ButtonStateChange()
            {
                // Start the animation
                yield return AnimateButtonGroup(AnimationDirection.In);
                
                // After the animation is finished, set EventSystem's current target
                // to the first Button in the ButtonGroup
                EventSystem.current.SetSelectedGameObject(_buttons[0].gameObject);
            }
        }

        /// Animates out the ButtonGroup
        public void HideButtons()
        {
            StartCoroutine(ButtonStateChange());

            IEnumerator ButtonStateChange()
            {
                // Start the animation
                yield return AnimateButtonGroup(AnimationDirection.Out);
                
                // Sets the ButtonGroup to inactive, after animating
                gameObject.SetActive(false);
                
                // After the menu is invisible, nullify EventSystem's current target
                EventSystem.current.SetSelectedGameObject(null);
                
                // Deselect all Buttons in this ButtonGroup
                foreach (var button in _buttons)
                {
                    button.SetButton(BaseUIButton.ButtonState.InActive);
                }
            }
        }

        /// Animates the entire ButtonGroup/Menu in/out with a linear fade
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

        public override void ShowButtonGroup<TB>(ButtonGroup<TB> bg)
        {
            throw new System.NotImplementedException();
        }
    }
}