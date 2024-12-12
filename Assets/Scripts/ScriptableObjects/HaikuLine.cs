using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class HaikuLine : ScriptableObject
    {
        // ReSharper disable once InconsistentNaming
        [NonSerialized] public int value = 0;
    }
}