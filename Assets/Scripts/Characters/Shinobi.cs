using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Shinobi : Character
    {
        [Header("Rise/Fall Speeds")]
        [SerializeField] private float _risingGravityScale = 5f;
        [SerializeField] private float _fallingGravityScale = 10f;
        private static readonly int Falling = Animator.StringToHash("ShouldFall");

        [Header("Jump & Attack Forces")]
        [SerializeField] private float _jumpUpwardForce = 350f;
        [SerializeField] private float _jumpForwardForce = 80f;
        private static readonly int Slashing = Animator.StringToHash("ShouldSlash");

        
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
        protected override string CharacterTitle() => "Shinobi/忍び";

        #region Animation Events

        [UsedImplicitly]
        public void OnJumpUp()
        {
            Rb2d.velocity = Vector2.zero;

            int direction = _playerType == PlayerType.One ? 1 : -1;
            Rb2d.AddForce(new Vector2(_jumpForwardForce * direction, 
            _jumpUpwardForce), ForceMode2D.Impulse);
        }

        [UsedImplicitly]
        public void OnFinishFall()
        {
            Anim.SetTrigger(Slashing);
        }
        public override void OnStrikeTarget(int isFinalHit)
        {
            Opponent.DoHurtAnimation(4, 0.45f);
            
            if (isFinalHit != 1) return;
            
            Opponent.DoDeathAnimation();
            base.OnStrikeTarget(isFinalHit);
        }
        
        public override void OnFinishAttack()
        {
            base.OnFinishAttack();
            Anim.ResetTrigger(Falling);
            Rb2d.gravityScale = _risingGravityScale;
        }

        #endregion
        
    }
}