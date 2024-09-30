using System.Collections;
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
        [SerializeField] private GameObject _gameCanvas;
        
        public static CanvasFader Fader { get; private set;  }
        
        private void Awake()
        {
            if (Fader != null && Fader != this)
            {
                Destroy(Fader);
                return;
            }

            Fader = this;
            DontDestroyOnLoad(gameObject);
        }
        
        private void OnEnable()
        {
            _cg = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeCanvas()
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
                // this.gameObject.SetActive(false);
                
                // _gameCanvas.SetActive(true);
        }

        public void SetUICanvasInactive() => this.gameObject.SetActive(false);

        public void SetGameCanvasActive() => _gameCanvas.SetActive(true);
    }
}