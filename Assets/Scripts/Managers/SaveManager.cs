using UI;
using UnityEngine;

namespace Managers
{
    public sealed class SaveManager : MonoBehaviour
    {
        #region Player Character Choice
        
        private const string PlayerCharacterKey = "Player Character";

        /// Saves the player's character choice (Ronin, Shogun, etc.)
        public static void SavePlayerCharacter(SamuraiType samuraiType)
        {
            PlayerPrefs.SetInt(PlayerCharacterKey, (int) samuraiType);
            PlayerPrefs.Save();
        }

        public static SamuraiType LoadPlayerCharacter() => (SamuraiType) 
        PlayerPrefs.GetInt(PlayerCharacterKey);

        #endregion
    }
}