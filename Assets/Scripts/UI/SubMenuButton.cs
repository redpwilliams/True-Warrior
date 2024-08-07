using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SubMenuButton : Button
    {
        [SerializeField] private GameObject _parentButton;
        
        public override void OnSubmit(BaseEventData eventData)
        {
            print("submitted this button");
        }

        public override void OnCancel(BaseEventData eventData)
        {
            print("cancelled this button");
            /*
             * Click off / escape - remove sub menu and deselect active main menu button
             * "clickOffCancel" <-- default
             * Cancel by moving left - remove sub menu but keep active main menu button
             * "inputLeftCancel"
             */
            StartCoroutine(WaitAndCheck());
        }

        protected override IEnumerator WaitAndCheck()
        {
            yield return null;

            EventManager.Events.SubMenuButtonCancel
                (EventSystem.current.currentSelectedGameObject is null 
                    ? null 
                    : _parentButton);
        }
    }
}