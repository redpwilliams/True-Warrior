using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Managers;
using GameMode = Managers.GameManager.GameMode;

namespace UI.Buttons
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public abstract class BaseUIButton : MonoBehaviour, ISelectHandler, 
        IDeselectHandler, ISubmitHandler, ICancelHandler
    {
        
        public enum ButtonState { Active, InActive }

        private ButtonState _buttonState;
        
        private readonly float _moveOffset = 15f;
        [Header("Child references")]
        [SerializeField] private GameObject _enChild;
        [SerializeField] private GameObject _jpChild;
        private RectTransform _en, _jp;

        private UnityEngine.UI.Button _button;

        protected virtual void OnEnable()
        {
            _en = _enChild.GetComponent<RectTransform>();
            _jp = _jpChild.GetComponent<RectTransform>();
            _button = GetComponent<UnityEngine.UI.Button>();
            _buttonState = ButtonState.InActive;
            EventManager.Events.OnDisableAllButtons += DisableButton;
        }

        private void OnDisable()
        {
            EventManager.Events.OnDisableAllButtons -= DisableButton;
        }

        /// Moves this button both outward/inward by a predetermined offset
        protected IEnumerator MoveButton(ButtonState state)
        {

            // Don't do anything if the state to move to is the current state
            if (state == _buttonState) yield break;
            
            // Set the current state
            _buttonState = state;
            
            float start = state == ButtonState.Active ? 0 : _moveOffset;
            float end = state == ButtonState.Active ? _moveOffset : 0;
            
            // Cache current positions
            var enLocalPosition = _en.localPosition;
            var jpLocalPosition = _jp.localPosition;
            
            // Create start position vectors
            Vector3 enStartPosition = new Vector3(start, enLocalPosition.y, enLocalPosition.z);
            Vector3 jpStartPosition = new Vector3(start, jpLocalPosition.y, jpLocalPosition.z);
            
            // Create end position vectors
            Vector3 enEndPosition = new Vector3(end, enLocalPosition.y, enLocalPosition.z);
            Vector3 jpEndPosition = new Vector3(end, jpLocalPosition.y, jpLocalPosition.z);

            float timeElapsed = 0f;
            float moveDuration = 0.1f;

            while (timeElapsed < moveDuration)
            {
                float t = timeElapsed / moveDuration;

                _en.localPosition = Vector3.Lerp(enStartPosition, enEndPosition, t);
                _jp.localPosition = Vector3.Lerp(jpStartPosition, jpEndPosition, t);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Snap to final positions after animation
            _en.localPosition = enEndPosition;
            _jp.localPosition = jpEndPosition;
        }

        /// Unlike BaseUIButton.MoveButton, this method sets the Button
        /// as active or inactive, without animating it.
        /// Thus, it is done outside of a coroutine.
        public void SetButton(ButtonState state)
        {
            // Do nothing if trying to set a button to the same state
            if (state == _buttonState) return;

            // Get button end position
            float end = state == ButtonState.Active  ? _moveOffset : 0;
            
            // Cache current positions
            var enLocalPosition = _en.localPosition;
            var jpLocalPosition = _jp.localPosition;
            
            // Express as Vector3
            Vector3 enEndPosition = new Vector3(end, enLocalPosition.y, enLocalPosition.z);
            Vector3 jpEndPosition = new Vector3(end, jpLocalPosition.y, jpLocalPosition.z);

            // Set Button's position to new position
            _en.localPosition = enEndPosition;
            _jp.localPosition = jpEndPosition;
        }

        /// Synchronous function to run after waiting
        protected delegate void CheckFunctionSync();

        /// Determines if it was a general deselect or a menu click-off, synchronously
        protected static IEnumerator WaitAndCheck(CheckFunctionSync functionSync)
        {
            yield return null;
            functionSync();
        }
        
        // Asynchronous function to run after waiting
        protected delegate IEnumerator CheckFunctionAsync();

        /// Determines if it was a general deselect or a menu click-off, synchronously
        protected static IEnumerator WaitAndCheck(CheckFunctionAsync checkFunction)
        {
            yield return null;
            yield return checkFunction();
        }

        private void DisableButton(GameMode gm) => _button.enabled = false;

        /// Starts the MoveButton Coroutine, moving the button outwards
        public virtual void OnSelect(BaseEventData eventData) => StartCoroutine(MoveButton(ButtonState.Active));

        /// Starts the MoveButton Coroutine, returning the button inwards
        public virtual void OnDeselect(BaseEventData eventData) => 
        StartCoroutine(MoveButton(ButtonState.InActive));

        /// Fires when this button is active and EventSystem captures a "submit" input
        public abstract void OnSubmit(BaseEventData eventData);

        /// Fires when this button is active and EventSystem captures a "cancel" input
        public abstract void OnCancel(BaseEventData eventData);

        // Fires when this button gets clicked
        // public abstract void OnPointerClick(PointerEventData pointerEventData);
    }
}