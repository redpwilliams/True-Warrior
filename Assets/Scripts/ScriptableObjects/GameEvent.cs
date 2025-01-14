using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName ="Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> _listeners = new();

        public void TriggerEvent()
        {
            for (int i = _listeners.Count -1; i >= 0; i--)
            {
                _listeners[i].OnEventTriggered();
            }
        }

        public void AddListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }
    }
}