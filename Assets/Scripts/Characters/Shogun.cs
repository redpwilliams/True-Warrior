using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Shogun : Character
    {
        [Header("Dash Attack Parameters")]
        [SerializeField] private float _dashDistance = 13f;
        [SerializeField, Min(0f)] private float _dashDuration = 0.5f;


        [UsedImplicitly]
        public void OnDashAttackStart() => StartCoroutine(Dash());

        private IEnumerator Dash()
        {
            // Move the character towards the final position
            Vector2 currentPosition = Trans.position;
            Vector2 targetPosition = currentPosition + new Vector2
                (GetDirection() * _dashDistance, currentPosition.y);

            float elapsedTime = 0f;
            
            while (elapsedTime < _dashDuration)
            {
                float t = elapsedTime / _dashDuration;

                var position = Vector2.Lerp(currentPosition, targetPosition, 
                1-Mathf.Pow(1-t,4));
                Rb2d.MovePosition(position);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Rb2d.MovePosition(targetPosition);
        }

        public override void OnStrikeTarget(int isFinalHit)
        {
            Opponent.DoHurtAnimation(0);

            if (isFinalHit != 1) return;

            Opponent.DoDeathAnimation();
            base.OnStrikeTarget(isFinalHit);
        }

    }
}