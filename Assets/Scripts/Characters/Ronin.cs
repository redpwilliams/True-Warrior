using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Ronin : Character, IReactive
    {
        [SerializeField] private float _risingGravityScale = 5f;
        [SerializeField] private float _fallingGravityScale = 10f;
        private static readonly int Falling = Animator.StringToHash("ShouldFall");

        
        protected override void Start()
        {
            base.Start();
            EventManager.Events.OnCharacterAttacks += ReactToAttack;
            Rb2d.gravityScale = _risingGravityScale;
        }

        // TODO - Must turn off _isRunning temporarily for this to work
        [UsedImplicitly]
        public void OnStrongAttackJumpUp()
        {
            Rb2d.velocity = Vector2.zero;
            Rb2d.bodyType = RigidbodyType2D.Dynamic; 
            Rb2d.AddForce(Vector2.up*400, ForceMode2D.Impulse);
        }

        private void Update()
        {
            if (!(Rb2d.velocity.y < 0f)) return;
            Anim.SetTrigger(Falling);
            Rb2d.gravityScale = _fallingGravityScale;
        }

        [UsedImplicitly]
        public void OnFinishAttack()
        {
            Anim.SetBool(Attacking, false);
            Anim.ResetTrigger(Falling);
            Rb2d.gravityScale = _risingGravityScale;
        }

        public void ReactToAttack()
        {
            throw new NotImplementedException();
        }
    
    }
}
