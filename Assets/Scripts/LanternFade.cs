using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LanternFade : MonoBehaviour
{
    private Light2D _light;
    [SerializeField] private float _oscillationFrequency = 1f;

    private void Start()
    {
        _light = GetComponent<Light2D>();
    }
    
    private void Update()
    {
        _light.intensity = Mathf.Lerp(0.8f, 1.5f, Mathf
        .PingPong(Time.time * _oscillationFrequency, 1f));
    }
}
