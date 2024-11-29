using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace UI.Buttons.Menu
{
    /// Groups the Main Menu buttons; specifically, the Play,
    /// Instructions, Options, and Quit Buttons
    public class MainMenuButtonGroup : ButtonGroup<MenuButton>
    {
        private readonly float _moveOffset = -200f;
        private RectTransform _rt;
        private bool _buttonIsSelected;
        private GameObject _activeSubMenu;

        protected override void Start()
        {
            base.Start();
            
            _rt = GetComponent<RectTransform>();
            // EventManager.Events.OnMenuButtonSubmit += HandleSubmit;
            EventManager.Events.OnMenuButtonCancel += HandleMenuCancel;
            EventManager.Events.OnSubMenuButtonCancel += HandleSubMenuCancel;
        }

        
        // protected override void ManagerHandshake()
        // {
        //     // Handshake
        //     foreach (var button in _buttons)
        //     {
        //         button.Manager = this;
        //     }
        // }
        
        public override void ShowButtonGroup<T>(ButtonGroup<T> bg)
        {
            SubMenuButtonGroup smbg = bg as SubMenuButtonGroup;
            if (_buttonIsSelected) return;
            _buttonIsSelected = true;
            
            StartCoroutine(MoveMenu(true, smbg!.gameObject, smbg.DefaultButton
            .gameObject));
        }
        
        private void OnDisable()
        {
            EventManager.Events.OnMenuButtonCancel -= HandleMenuCancel;
            EventManager.Events.OnSubMenuButtonCancel -= HandleSubMenuCancel;
        }

        private void HandleMenuCancel(GameObject subMenu)
        {
            // Prevent menu from jumping to active, then transition to inactive
            if (!_buttonIsSelected) return;
            StartCoroutine(MoveMenu(false, subMenu, null));
            _buttonIsSelected = false;
        }

        private void HandleSubMenuCancel(GameObject nowActiveButton)
        {
            _buttonIsSelected = false;
            StartCoroutine(MoveMenu(false, _activeSubMenu, nowActiveButton));
        }

        private IEnumerator MoveMenu(bool isSubmit, GameObject subMenu, 
            GameObject nowActiveButton)
        {
            // Remove sub menu before animating main menu move in
            if (!isSubmit)
            { 
                _activeSubMenu.SetActive(false);
                _activeSubMenu = null;
                EventSystem.current.SetSelectedGameObject(nowActiveButton);
            }
            
            float start = isSubmit ?  0 : _moveOffset;
            float end = isSubmit  ? _moveOffset : 0;

            var localPosition = _rt.localPosition;

            Vector3 startPosition = new Vector3(start, localPosition.y, localPosition.z);
            Vector3 endPosition = new Vector3(end, localPosition.y, localPosition.z);

            float timeElapsed = 0;
            float moveDuration = 0.1f;

            while (timeElapsed < moveDuration)
            {
                float t = timeElapsed / moveDuration;
                _rt.localPosition = Vector3.Lerp(startPosition, endPosition,
                    Lerp2D.EaseOutQuart(t));

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _rt.localPosition = endPosition;
            
            // Show sub menu after animating main menu move out
            if (!isSubmit) yield break;
            subMenu.SetActive(true);
            _activeSubMenu = subMenu;
            EventSystem.current.SetSelectedGameObject(nowActiveButton);
        }
    }
}