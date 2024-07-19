using Managers;
using UnityEditor;

public class MenuItems : Editor
{
    private const string EventsMenu = "Developer Events/";
    private const string BeginAttackPath = EventsMenu + "Trigger BeginAttack";

    #region BeginAttack

    [MenuItem(BeginAttackPath)]
    private static void BeginAttack()
    {
        EventManager.Events.BeginAttack();
    }

    #endregion
}
