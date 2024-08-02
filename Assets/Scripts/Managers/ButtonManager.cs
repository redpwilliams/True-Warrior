using UnityEngine;

namespace Managers
{
    public class ButtonManager : MonoBehaviour
    {
        /// Singleton Instance
        public static ButtonManager Buttons { get; private set; }

        private void Awake()
        {
            if (Buttons != null && Buttons != this)
            {
                Destroy(Buttons);
                return;
            }

            Buttons = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}