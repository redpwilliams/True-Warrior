using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Characters
{
    public class Shinobi : Character
    {
        [Header("Rise/Fall Speeds")]

        private SpriteRenderer _sr;
        private Light2D _light;
        
        protected override void Start()
        {
            base.Start();
            _sr = GetComponent<SpriteRenderer>();
            _light = GetComponent<Light2D>();
        }

        protected override string CharacterTitle() => "Shinobi/忍び";

        #region Animation Events

        [UsedImplicitly]
        protected override void Attack()
        {
            Rb2d.velocity = Vector2.zero;
            StartCoroutine(SneakStrike(true));
        }

        private IEnumerator SneakStrike(bool shouldDisappear)
        {
            // Sprite alpha
            float startAlpha = shouldDisappear ? 1 : 0;
            float endAlpha = shouldDisappear ? 0 : 1;
            Color sc = _sr.material.color;
                
            // Light2D intensity
            float initialIntensity = _light.intensity;
            float targetFadeIntensity = 0.3f;
            float startIntensity = shouldDisappear ? initialIntensity : targetFadeIntensity;
            float endIntensity = shouldDisappear ? targetFadeIntensity : initialIntensity;

            float animationDuration = 0.5f;
            float elapsedTime = 0f;
                
            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;
                    
                // Animate sprite alpha
                _sr.material.color =
                    new Color(sc.r, sc.g, sc.b, Mathf.Lerp(startAlpha, 
                        endAlpha, Lerp2D.EaseOutExpo(t)));
                    
                // Animate sprite light intensity
                _light.intensity = Mathf.Lerp(startIntensity,
                    endIntensity, Lerp2D.EaseOutExpo(t));

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
                CharText.Flip();       
                
                base.Attack(); // Init attack and slash animation
                yield break;
            }
            
            // Wait for QuickMove to finish
            float moveDistance = InitParams.EndPositionX * 2 + 2.25f;
            float moveDuration = 0.2f;
            yield return QuickMove(moveDistance, moveDuration, function: Lerp2D.EaseInQuart);

            // Re-run to reappear
            yield return SneakStrike(false);
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