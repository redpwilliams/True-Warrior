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
        [SerializeField] private HaikuText _ht;
        [SerializeField] private GameObject _gameCanvas;
        
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

        private void FadeCanvas(GameMode gm)
        {
            StartCoroutine(Fade());

            IEnumerator Fade()
            {
                float animationDuration = 1.5f;
                float elapsedTime = 0;

                while (elapsedTime < animationDuration)
                {
                    float t = elapsedTime / animationDuration;
                    _cg.alpha = Mathf.Lerp(1, 0, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _cg.alpha = 0;
                this.gameObject.SetActive(false);
                
                _gameCanvas.SetActive(true);
                _ht.StartGameMode(gm);
            }
        }
    }
}