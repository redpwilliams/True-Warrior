using System.Collections;
using Managers;
using UnityEngine;

namespace UI
{
    /*
     * TODO: Rework game start sequence
     * 1. At the same time, the screen should dim to black and *just*
     * the menu should fade out. The title should remain. The
     * lamps and maybe
     * 2. After some time, the title fades out too.
     * 3. Lights 
     */
    
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
            EventManager.Events.OnGameModeSelected += FadeCanvas;
        }

        private void OnDisable()
        {
            EventManager.Events.OnGameModeSelected -= FadeCanvas;
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