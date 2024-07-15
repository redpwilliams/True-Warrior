using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    [SerializeField] protected int targetWidth = 1920;
    [SerializeField] protected int targetHeight = 1080;

    [SerializeField] protected int dynamicMaxWidth = 2560;
    [SerializeField] protected int dynamicMaxHeight = 1440;

    [SerializeField] protected bool useDynamicWidth = false;
    [SerializeField] protected bool useDynamicHeight = false;

    private Camera camera;
    private int lastWidth = 0;
    private int lastHeight = 0;

    private float orthoSize;

    protected void Awake()
    {
        camera = GetComponent<Camera>();
        orthoSize = camera.orthographicSize;
    }

    protected void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateCamSize();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    private void UpdateCamSize()
    {
        float targetaspect = 16.0f / 9.0f;
 
        float windowaspect = (float)Screen.width / (float)Screen.height;
 
        float scaleheight = windowaspect / targetaspect;
 
 
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;
 
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
 
            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;
 
            Rect rect = camera.rect;
 
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
 
            camera.rect = rect;
        }

    }
}
