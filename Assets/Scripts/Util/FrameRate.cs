using UnityEngine;

public class FrameRate : MonoBehaviour
{
    private readonly int _frameRate = 60;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _frameRate;   
    }
}
