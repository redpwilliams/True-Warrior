using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Shogun : Character
    {
        [SerializeField] private float _strikePushForce = 200f;
        
        [UsedImplicitly]
        public void OnDashAttackStart()
        {
            Rb2d.velocity = Vector2.zero;

            int direction = _playerType == PlayerType.One ? 1 : -1;
            Rb2d.AddForce(new Vector2(direction * 200f, 0f), ForceMode2D.Impulse);
        }

        public override void OnStrikeTarget(int isFinalHit)
        {
            // AnimationEvent cannot accept booleans
            int direction = _playerType == PlayerType.One ? 1 : -1;
            Opponent.DoHurtAnimation(direction * _strikePushForce);
            
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