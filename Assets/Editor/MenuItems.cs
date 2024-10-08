using System;
using Managers;
using UnityEditor;
using Characters;
using JetBrains.Annotations;
using UI;
using UI.Buttons;
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

    private const string PrintSavedCharacter =
        SaveActionsSubMenu + "Print Saved Character";

    private const string SetSavedCharacter =
        SaveActionsSubMenu + "Set Saved Character/";


    [SerializeField] private int _test;
    // private static MenuButton _optionsButton;
    // private static OptionsMenuButton _roninButton;
    // private static OptionsMenuButton _shinobiButton;
    // private static OptionsMenuButton _shogunButton;
    private const string Ronin = SetSavedCharacter + "Ronin";
    private const string Shinobi  = SetSavedCharacter + "Shinobi ";
    private const string Shogun = SetSavedCharacter + "Shogun";

    private void OnEnable()
    {
        Debug.Log("Test");
    }

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

    [MenuItem(PrintSavedCharacter)]
    private static void PrintSavedSamurai()
    {
        SamuraiType st = SaveManager.LoadPlayerCharacter();
        Debug.Log(st);
    }

    [MenuItem(Ronin)]
    private static void SetRonin()
    {
        EventManager.Events.DeselectAllChosenTOs();
        SaveManager.SavePlayerCharacter(SamuraiType.Ronin);
    }

    [MenuItem(Shinobi)]
    private static void SetShinobi()
    {
        EventManager.Events.DeselectAllChosenTOs();
        SaveManager.SavePlayerCharacter(SamuraiType.Shinobi);
    }

    [MenuItem(Shogun)]
    private static void SetShogun(MenuCommand mc)
    {
        EventManager.Events.DeselectAllChosenTOs();
        SaveManager.SavePlayerCharacter(SamuraiType.Shogun);
        // _optionsButton.SetFirstButton();
    }
    
    #endregion
}
