using Managers;
using UnityEditor;
using Characters;
using JetBrains.Annotations;
using UI;
using UnityEngine;

public class MenuItems : Editor
{
    /// The title/overall custom menu
    private const string DeveloperActionsMenu = "Developer Actions/";
    
    private const string BeginAttackPath = 
        DeveloperActionsMenu + "Trigger BeginAttack";
    
    private const string SetCharacterPositioning =
        DeveloperActionsMenu + "Set Character Positions";

    private const string SaveActionsSubMenu =
        DeveloperActionsMenu + "Save\\Load Actions/";

    private const string PrintSavedSamuraiType =
        SaveActionsSubMenu + "Print Saved Samurai Type";

    #region BeginAttack

    [MenuItem(BeginAttackPath)]
    private static void BeginAttack()
    {
        EventManager.Events.BeginAttack();
    }

    #endregion

    #region Character Positioning

    [UsedImplicitly, MenuItem(SetCharacterPositioning)]
    private static void SetCharacterPositions()
    {
        Character[] characters = FindObjectsByType<Character>(FindObjectsSortMode.None);
        foreach (var character in characters)
        {
            character.SetCharacterStartPosition();
        }
    }

    #endregion

    #region Save/Load

    [MenuItem(PrintSavedSamuraiType)]
    private static void PrintSavedSamurai()
    {
        SamuraiType st = SaveManager.LoadPlayerCharacter();
        Debug.Log(st);
    }

    #endregion
}
