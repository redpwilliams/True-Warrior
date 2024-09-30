using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    { 
        public enum GameMode { Standoff, Survival, Zen }
        
        /// Singleton Instance
        public static GameManager Manager { get; private set; }

        private void Awake()
        {
            if (Manager != null && Manager != this)
            {
                Destroy(Manager);
                return;
            }

            Manager = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartGameMode(GameMode gm)
        {
        }
    }
}