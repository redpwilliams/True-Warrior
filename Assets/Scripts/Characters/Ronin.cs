using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Ronin : Character
    {
        [Header("Rise/Fall Speeds")]
        [SerializeField] private float _risingGravityScale = 5f;
        [SerializeField] private float _fallingGravityScale = 10f;
        private static readonly int Falling = Animator.StringToHash("ShouldFall");

        [Header("Jump & Strike Forces")]
        [SerializeField] private float _jumpUpwardForce = 400f;
        [SerializeField] private float _jumpForwardForce = 100f;

        
        protected override void Start()
        {
            base.Start();
            Rb2d.gravityScale = _risingGravityScale;
        }

        private void Update()
        {
            if (!(Rb2d.velocity.y < 0f)) return;
            Anim.SetTrigger(Falling);
            Rb2d.gravityScale = _fallingGravityScale;
        }

        #region AnimationEvent

        [UsedImplicitly]
        public void OnStrongAttackJumpUp()
        {
            Rb2d.velocity = Vector2.zero;

            int direction = _playerType == PlayerType.One ? 1 : -1;
            Rb2d.AddForce(new Vector2(_jumpForwardForce * direction, 
            _jumpUpwardForce), ForceMode2D.Impulse);
        }
        
        public override void OnFinishAttack()
        {
            base.OnFinishAttack();
            Anim.ResetTrigger(Falling);
            Rb2d.gravityScale = _risingGravityScale;
        }

        public override void OnStrikeTarget(int isFinalHit)
        {
            Opponent.DoHurtAnimation(5, 0.5f);
            
            if (isFinalHit != 1) return;
            
            Opponent.DoDeathAnimation();
            base.OnStrikeTarget(isFinalHit);
        }


        #endregion

    }
}
