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
    public class UICanvas : MonoBehaviour
    {
        private CanvasGroup _cg;
        [SerializeField] private GameObject _gameCanvas;
        
        public static UICanvas Canvas { get; private set;  }
        
        private void Awake()
        {
            if (Canvas != null && Canvas != this)
            {
                Destroy(Canvas);
                return;
            }

            Canvas = this;
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
        }

        public void SetUICanvasInactive() => this.gameObject.SetActive(false);

        public void SetGameCanvasActive() => _gameCanvas.SetActive(true);
    }
}