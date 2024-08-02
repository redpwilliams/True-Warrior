using UI;
using UnityEngine;

namespace Managers
{
    public class ButtonManager : MonoBehaviour
    {
        private void Start()
        {
            EventManager.Events.OnMenuButtonSubmit += HandleSubmit;
        }

        private void HandleSubmit(MenuButton.ButtonClass type) => print(type);
    }
}