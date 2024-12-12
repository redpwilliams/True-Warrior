using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent _gameEvent;
        public UnityEvent _onEventTriggered;

        private void OnEnable()
        { 
            _gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            _gameEvent.RemoveListener(this);
        }

        public void OnEventTriggered()
        {
            _onEventTriggered.Invoke();
        }
    }
}