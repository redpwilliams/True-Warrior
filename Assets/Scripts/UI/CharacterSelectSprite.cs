using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Light2D))]
public class CharacterSelectSprite : MonoBehaviour
{
    private Animator _anim;
    private Light2D _light;

    private readonly float _lightOnIntensity = 0.75f;
    private readonly float _lightOffIntensity = 0.5f;
    
    private void OnEnable()
    {
        _anim = GetComponent<Animator>();
        _anim.enabled = false;

        _light = GetComponent<Light2D>();
        DimSpriteLight();
    }

    public void StopAnimation() => _anim.enabled = false;

    public void StartAnimation() => _anim.enabled = true;

    public void DimSpriteLight() => _light.intensity = _lightOffIntensity;

    public void BrightenSpriteLight() => _light.intensity = _lightOnIntensity;
}
