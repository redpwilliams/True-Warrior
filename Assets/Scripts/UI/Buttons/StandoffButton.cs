using GameMode = Managers.GameManager.GameMode;
using UnityEngine;

namespace UI.Buttons
{
    public class StandoffButton : PlayMenuButton
    {
        [SerializeField] private HaikuText _ht;
        
        protected override void StartGameMode()
        {
          _ht.StartGameMode(GameMode.Standoff);  
        }
    }
}