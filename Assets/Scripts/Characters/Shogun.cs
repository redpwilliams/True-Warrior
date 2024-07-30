using JetBrains.Annotations;
using UnityEngine;

namespace Characters
{
    public class Shogun : Character
    {
        [Header("Dash Attack Parameters")]
        [SerializeField] private float _dashDistance = 13f;
        [SerializeField, Min(0f)] private float _dashDuration = 0.5f;

        protected override string CharacterTitle() => "Shogun/将軍";

        [UsedImplicitly]
        public void OnDashAttackStart() =>
            StartCoroutine(QuickMove(_dashDistance, _dashDuration));

        public override void OnStrikeTarget(int isFinalHit)
        {
            Opponent.DoHurtAnimation();

            if (isFinalHit != 1) return;

            Opponent.DoDeathAnimation();
            base.OnStrikeTarget(isFinalHit);
        }

    }
}