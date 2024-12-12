using System;
using PlayerNumber = Characters.PlayerNumber;
using Util;

// ReSharper disable InconsistentNaming

namespace Managers
{
    /*
     * To future me:
     * I'm trying out this idea of an EventHub. I want the
     * GameManager to be the central point for everything.
     * All scripts will look to that. No more EventManager singleton.
     * I was thinking to have all events be public - anyone
     * can subscribe to it. But, only the 
     */
    public partial class GameManager
    {
        // public static event Action<int> Standoff_OnStageStarted;
        // public static event Action<int> Standoff_OnStageFinished;
        public static event Action<PlayerNumber, PlayerInputFlags> OnEnablePlayerControls;
        public static event Action<PlayerNumber, PlayerInputFlags> OnDisablePlayerControls;

        // private static void Standoff_StageStarted(int stage)
        // {
        //     Standoff_OnStageStarted?.Invoke(stage);
        // }
        //
        // private static void Standoff_StageFinished(int stage)
        // {
        //     Standoff_OnStageFinished?.Invoke(stage);
        // }

        private static void EnablePlayerControls(PlayerNumber playerNumber,
            PlayerInputFlags flags)
        {
            OnEnablePlayerControls?.Invoke(playerNumber, flags);
        }
        
        private static void DisablePlayerControls(PlayerNumber playerNumber,
            PlayerInputFlags flags)
        {
            OnDisablePlayerControls?.Invoke(playerNumber, flags);
        }
    }
}