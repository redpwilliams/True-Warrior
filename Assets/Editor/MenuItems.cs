using Managers;
using UnityEditor;
using UI;
using UnityEngine;

public class MenuItems : Editor
{
    /// The title/overall custom menu
    private const string DeveloperActionsMenu = "Developer Actions/";
    
    private const string SaveActionsSubMenu =
        DeveloperActionsMenu + "Save\\Load Actions/";

    private const string PrintSavedCharacter =
        SaveActionsSubMenu + "Print Saved Character";

    private const string SetSavedCharacter =
        SaveActionsSubMenu + "Set Saved Character/";

    // private static MenuButton _optionsButton;
    // private static OptionsMenuButton _roninButton;
    // private static OptionsMenuButton _shinobiButton;
    // private static OptionsMenuButton _shogunButton;
    private const string Ronin = SetSavedCharacter + "Ronin";
    private const string Shinobi  = SetSavedCharacter + "Shinobi ";
    private const string Shogun = SetSavedCharacter + "Shogun";

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
