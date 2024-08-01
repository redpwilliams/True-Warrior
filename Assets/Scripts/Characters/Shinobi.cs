using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Characters
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Light2D))]
    public class Shinobi : Character
    {
        private SpriteRenderer _sr;
        private Light2D _light;
        private CharacterText _ct;

        private float _initialIntensity;
        
        protected override void Start()
        {
            base.Start();
            _sr = GetComponent<SpriteRenderer>();
            _light = GetComponent<Light2D>();
            _ct = GetComponentInChildren<CharacterText>();
            
            _initialIntensity = _light.intensity;
        }

        protected override string CharacterTitle() => "Shinobi/忍び";

        #region Animation Events

        [UsedImplicitly]
        protected override void Attack()
        {
            Rb2d.velocity = Vector2.zero;
            StartCoroutine(SneakStrike(true));
        }

        private IEnumerator SneakStrike(bool shouldDisappear, Lerp2DFunction 
        function = null)
        {
            // Handle null easing function
            function ??= Lerp2D.NoEase;
            
            // Sprite alpha
            float startAlpha = shouldDisappear ? 1 : 0;
            float endAlpha = shouldDisappear ? 0 : 1;
            Color sc = _sr.material.color;
                
            // Light2D intensity
            float targetFadeIntensity = 0.3f;
            float startIntensity = shouldDisappear ? _initialIntensity : targetFadeIntensity;
            float endIntensity = shouldDisappear ? targetFadeIntensity : _initialIntensity;

            float animationDuration = 0.12f;
            float elapsedTime = 0f;
                
            // Start QuickMove 
            if (shouldDisappear)
            {
                float moveDistance = InitParams.EndPositionX * 2 + 2.25f;
                float moveDuration = 0.2f;
                StartCoroutine(QuickMove(moveDistance, moveDuration,
                    function: Lerp2D.NoEase));
            }

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                    
                // Animate sprite alpha
                _sr.material.color =
                    new Color(sc.r, sc.g, sc.b, Mathf.Lerp(startAlpha, 
                        endAlpha, function(t)));
                    
                // Animate sprite light intensity
                _light.intensity = Mathf.Lerp(startIntensity,
                    endIntensity, function(t));

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Snap to final values after animation
            _sr.material.color = new Color(sc.r, sc.g, sc.b, endAlpha);
            _light.intensity = endIntensity;
            
            // shouldDisappear=false means SmokeBall was called from the call below
            // QuickMove shouldn't be called again
            if (!shouldDisappear)
            {
                // Turn around and attack
                var cachedTransform = transform;
                Vector3 localScale = cachedTransform.localScale;
                
                cachedTransform.localScale = new Vector3(
                    localScale.x * -1, localScale.y, localScale.z);

                // Flip child character text object
                _ct.Flip();       
                
                base.Attack(); // Init attack and slash animation
                yield break;
            }
            
            // Re-run to reappear
            yield return SneakStrike(false, Lerp2D.EaseOutBack);
            // TODO - Start Attacking while reappearing
        } 

        public override void OnStrikeTarget(int isFinalHit)
        {
            // Negative to push opponent forward instead of standard knockback
            Opponent.DoHurtAnimation(-4, 0.45f); 
            
            if (isFinalHit != 1) return;
            
            Opponent.DoDeathAnimation();
            base.OnStrikeTarget(isFinalHit);
        }
        
        #endregion
        
    }
}