using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterSelectSprite : MonoBehaviour
{
    private Animator _anim;
    
    private void OnEnable()
    {
        _anim = GetComponent<Animator>();
        _anim.enabled = false;
    }

    public void StopAnimation() => _anim.enabled = false;

    public void StartAnimation() => _anim.enabled = true;
}
