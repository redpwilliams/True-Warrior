using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Util
{
    public class InputEventListener : MonoBehaviour
    {
        public InputEvent _inputEvent;
        public UnityEvent<InputAction.CallbackContext> _onEventTriggered;

        private void OnEnable()
        { 
            _inputEvent.AddListener(this);
        }

        private void OnDisable()
        {
            _inputEvent.RemoveListener(this);
        }

        public void OnEventTriggered(InputAction.CallbackContext ctx)
        {
            _onEventTriggered.Invoke(ctx);
        }
    }
}