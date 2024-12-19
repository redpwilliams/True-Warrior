using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName ="Events/Input Event")]
    public class InputEvent : ScriptableObject
    {
        private readonly List<InputEventListener> _listeners = new();

        public void TriggerEvent(InputAction.CallbackContext ctx)
        {
            for (int i = _listeners.Count -1; i >= 0; i--)
            {
                _listeners[i].OnEventTriggered(ctx);
            }
        }

        public void AddListener(InputEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(InputEventListener listener)
        {
            _listeners.Remove(listener);
        }
        
    }
}