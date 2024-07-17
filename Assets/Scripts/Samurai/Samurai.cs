using JetBrains.Annotations;

public class Samurai : Character, IReactive
{
    protected override void Attack()
    {
        base.Attack();
    }

    public void OnFinishAttack()
    {
        throw new System.NotImplementedException();
    }
    
    public void ReactToAttack()
    {
        throw new System.NotImplementedException();
    }
}