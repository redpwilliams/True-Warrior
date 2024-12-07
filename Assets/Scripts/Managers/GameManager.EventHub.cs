using System;

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
            public static event Action<int> OnStandoffStageX;

            private static void StandoffStageX(int stage)
            {
                OnStandoffStageX?.Invoke(stage);
            }
    }
}