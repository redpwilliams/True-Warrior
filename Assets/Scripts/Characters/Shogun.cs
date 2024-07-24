using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Shogun : Character
    {
        
        [UsedImplicitly]
        public void OnDashAttackStart()
        {

            int direction = (int) Mathf.Sign(Trans.localScale.x);
            Rb2d.velocity = new Vector2(direction * 50f, 0);
            // // Move the character towards the final position
            // Vector2 currentPosition = Rb2d.position;
            // Vector2 targetPosition = new Vector2(_finalPosition, currentPosition.y);
            // Rb2d.MovePosition(Vector2.MoveTowards(currentPosition, targetPosition,
            //     _speed * Time.fixedDeltaTime));
            //
            // // Check if the character has reached or passed the final position
            // if ((_playerType == PlayerType.One && Rb2d.position.x < _finalPosition) || 
            //     _playerType != PlayerType.One && Rb2d.position.x > _finalPosition) return;
            //
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