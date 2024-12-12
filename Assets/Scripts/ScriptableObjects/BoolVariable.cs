using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Types/BoolVariable")]
    public class BoolVariable : ScriptableObject
    {
        public bool _value;
    }
}
