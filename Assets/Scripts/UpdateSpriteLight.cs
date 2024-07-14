using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UpdateSpriteLight : MonoBehaviour
{
   private SpriteRenderer _sr;
   private Light2D _light;

   public void Start()
   {
      _sr = gameObject.GetComponent<SpriteRenderer>();
      _light = gameObject.GetComponent<Light2D>();
   }

   public void UpdateLight2D()
   {
      Sprite sprite = _sr.sprite;

      if (_light != null)
      {
         // _light.lightCookieSprite = sprite;
      }
   } 
}
