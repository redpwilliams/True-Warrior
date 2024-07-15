using UnityEngine;

public class Ronin : Character
{
    [SerializeField] private float _gravityScale = 5f;
        
    protected override void Start()
    {
        base.Start();
        EventManager.Events.OnBeginAttack += BeginAttack;
        Rb2d.gravityScale = _gravityScale;
    }

    private void BeginAttack()
    {
       Debug.Log("Beginning attack"); 
       Anim.SetBool(Attacking, true);
    }

    // TODO - Must turn off _isRunning temporarily for this to work
    public void OnStrongAttackJumpUp()
    {
        Rb2d.velocity = Vector2.zero;
        Rb2d.bodyType = RigidbodyType2D.Dynamic; 
        Rb2d.AddForce(Vector2.up*400, ForceMode2D.Impulse);
    }

    public void OnStrikeDown()
    {
        Rb2d.velocity = Vector2.zero;
        Rb2d.AddForce(Vector2.down*100, ForceMode2D.Impulse);
        
    }

    public void OnAttackFinish()
    {
        
       Anim.SetBool(Attacking, false);
    }
    
}
