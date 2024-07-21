using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Shogun : Character
    {
        
        [UsedImplicitly]
        public void OnDashAttackStart()
        {

            int direction = _playerType == PlayerType.One ? 1 : -1;
            Rb2d.velocity = new Vector2(direction * 50f, 0);
        }

        public override void OnStrikeTarget(int isFinalHit)
        {
            Opponent.DoHurtAnimation(0);
            
            if (isFinalHit != 1) return;
            
            Opponent.DoDeathAnimation();
            base.OnStrikeTarget(isFinalHit);
        }

        public override void OnFinishAttack()
        {
            Anim.SetBool(Attacking, false);
        }
    }
}