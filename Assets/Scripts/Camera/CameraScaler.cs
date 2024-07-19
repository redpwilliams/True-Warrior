using UnityEngine;

// Referenced from:
// https://github.com/wmjoers/CameraScaler/blob/main/Assets/Scripts/CameraScaler.cs

namespace Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraScaler : MonoBehaviour
    {
        [SerializeField] protected int _targetWidth = 1920;
        [SerializeField] protected int _targetHeight = 1080;

        [SerializeField] protected int _dynamicMaxWidth = 2560;
        [SerializeField] protected int _dynamicMaxHeight = 1440;

        [SerializeField] protected bool _useDynamicWidth;
        [SerializeField] protected bool _useDynamicHeight;

        private UnityEngine.Camera _cam;
        private int _lastWidth;
        private int _lastHeight;

        private float _orthoSize;

        protected void Awake()
        {
            _cam = GetComponent<UnityEngine.Camera>();
            _orthoSize = _cam.orthographicSize;
        }

        protected void Update()
        {
            if (Screen.width == _lastWidth && Screen.height == _lastHeight) return;
            UpdateCamSize();
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
        }

        private void UpdateCamSize()
        {
            float targetAspect;
            float screenAspect = Screen.width / (float)Screen.height;
            float orthoScale = 1f;

            if (_useDynamicWidth)
            {
                float minTargetAspect = _targetWidth / (float)_targetHeight;
                float maxTargetAspect = _dynamicMaxWidth / (float)_targetHeight;
                targetAspect = Mathf.Clamp(screenAspect, minTargetAspect, maxTargetAspect);
            }
            else
            {
                targetAspect = _targetWidth / (float)_targetHeight;
            }

            float scaleValue = screenAspect / targetAspect;

            Rect rect = new();
            if (scaleValue < 1f)
            {
                if (_useDynamicHeight)
                {
                    float minTargetAspect = _targetWidth / (float)_dynamicMaxHeight;
                    if (screenAspect < minTargetAspect)
                    {
                        scaleValue = screenAspect / minTargetAspect;
                        orthoScale = minTargetAspect / targetAspect;
                    }
                    else
                    {
                        orthoScale = scaleValue;
                        scaleValue = 1f;
                    }
                }

                rect.width = 1;
                rect.height = scaleValue;
                rect.x = 0;
                rect.y = (1 - scaleValue) / 2;
            }
            else
            {
                scaleValue = 1 / scaleValue;
                rect.width = scaleValue;
                rect.height = 1;
                rect.x = (1 - scaleValue) / 2;
                rect.y = 0;
            }

            _cam.orthographicSize = _orthoSize / orthoScale;
            _cam.rect = rect;
        }
    }
}
