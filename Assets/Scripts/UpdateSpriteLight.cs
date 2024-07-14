using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UpdateSpriteLight : MonoBehaviour
{
   private SpriteRenderer _sr;
   private Sprite _sprite;
   private Light2D _light;

   private void Start()
   {
      _sr = gameObject.GetComponent<SpriteRenderer>();
      _light = gameObject.GetComponent<Light2D>();
   }

   private void LateUpdate()
   {
      _sprite = _sr.sprite;
   }

   public void UpdateLight2D()
   {
         _light.lightCookieSprite = _sprite;
         Debug.Log(_sprite);
   } 
}
