using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    { 
        public enum GameMode { Standoff, Survival, Zen }

        /// List of all haikus defined in "haiku.json"
        private List<Haiku> _haikus;
        
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
            _haikus = JsonReader.LoadHaikus();
            DontDestroyOnLoad(gameObject);
        }

        public void StartGameMode(GameMode gm)
        {
            switch (gm)
            {
                case GameMode.Standoff: 
                    // StartCoroutine(HaikuCountdown(_haikus));
                    break;
                case GameMode.Survival:
                    break;
                case GameMode.Zen:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gm), gm, null);
            }
        }
    }
}