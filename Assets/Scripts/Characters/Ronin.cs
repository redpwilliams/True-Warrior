using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Ronin : Character
    {
        [SerializeField] private float _risingGravityScale = 5f;
        [SerializeField] private float _fallingGravityScale = 10f;
        private static readonly int Falling = Animator.StringToHash("ShouldFall");

        [SerializeField] private float _jumpUpwardForce = 400f;
        [SerializeField] private float _jumpForwardForce = 100f;
        [SerializeField] private float _strikePushForce = 200f;

        
        protected override void Start()
        {
            base.Start();
            // EventManager.Events.OnCharacterAttacks += ReactToAttack;
            Rb2d.gravityScale = _risingGravityScale;
        }

        private void Update()
        {
            if (!(Rb2d.velocity.y < 0f)) return;
            Anim.SetTrigger(Falling);
            Rb2d.gravityScale = _fallingGravityScale;
        }

        protected override void Attack()
        {
            Anim.SetBool(Attacking, true);
        }

        protected override void LostToAttack()
        {
        }

        #region AnimationEvent

        // TODO - Must turn off _isRunning temporarily for this to work
        [UsedImplicitly]
        public void OnStrongAttackJumpUp()
        {
            Rb2d.velocity = Vector2.zero;

            int direction = _playerType == PlayerType.One ? 1 : -1;
            Rb2d.AddForce(new Vector2(_jumpForwardForce * direction, 
            _jumpUpwardForce), ForceMode2D.Impulse);
        }
        
        [UsedImplicitly]
        public void OnFinishAttack()
        {
            Anim.SetBool(Attacking, false);
            Anim.ResetTrigger(Falling);
            Rb2d.gravityScale = _risingGravityScale;
        }

        [UsedImplicitly]
        public void OnStrikeTarget(int isFinalHit)
        {
            // AnimationEvent cannot accept booleans
            int direction = _playerType == PlayerType.One ? 1 : -1;
            Opponent.DoHurtAnimation(direction * _strikePushForce);
            if (isFinalHit == 1)
            {
                Opponent.DoDeathAnimation();
                StartCoroutine(ReturnToIdle(3f));
            }
        }

        private IEnumerator ReturnToIdle(float time)
        {
            yield return new WaitForSeconds(time);
            Anim.SetTrigger(Return);
        }

        #endregion

    }
}
