using UnityEngine;

namespace Characters
{
    public class InitParams : ScriptableObject
    {
        // Movement & Initial Positions
        public const float StartPositionX = 25f;
        public const float StartPositionY = -9.1f;
        public const float StartPositionZ = 1;
        public const float EndPositionX = 3.5f;
        public const float RunSpeed = 15f;
        
        // SpriteLight2D
        public const float Light2DBright = 0.75f;
        public const float Light2DDim = 0.35f;
        public const float DimDuration = 1.25f;
    }
}