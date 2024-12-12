using System;
using UnityEngine;
using Util;

namespace ScriptableObjects
{
    /// The current stage the haiku is at
    [CreateAssetMenu]
    public class HaikuStage : ScriptableObject
    {
        // ReSharper disable once InconsistentNaming
        [NonSerialized] public StandoffState state = StandoffState.Pending;
    }
}