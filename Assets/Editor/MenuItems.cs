using Managers;
using UnityEditor;
using Characters;
using JetBrains.Annotations;
using UnityEngine;

public class MenuItems : Editor
{
    private const string EventsMenu = "Developer Actions/";
    private const string BeginAttackPath = EventsMenu + "Trigger BeginAttack";
    private const string SetCharacterPositioning =
        EventsMenu + "Set Character Positions";

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
}
