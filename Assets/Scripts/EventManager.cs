using System;
using UnityEngine;

/// <summary>Handles all events in the game</summary>
/// <remarks>
///     EventManager provides implementation for the subscription and dispatching of
///     all events in the game. <para />
///     To subscribe to an event,
///     <code>
///         EventManager.events.EventAction += MyMethod
///     </code>
///     <para />
///     To dispatch an event,
///     <code>
///         EventManager.events.OnYourEvent()
///     </code>
/// </remarks>
public sealed class EventManager : MonoBehaviour
{
    /// Singleton Instance
    public static EventManager Events { get; private set; }

    private void Awake()
    {
        if (Events != null && Events != this)
        {
            Destroy(Events);
            return;
        }

        Events = this;
        DontDestroyOnLoad(gameObject);
    }

    public event Action OnBeginAttack;
    public void BeginAttack()
    {
        OnBeginAttack?.Invoke();
    }

    #region Staging Logic

    private double _battleStartTime;
    public event Action<int> OnStageX;
    public void StageX(int stage)
    {
        if (stage == 3) _battleStartTime = Time.realtimeSinceStartupAsDouble;
        OnStageX?.Invoke(stage);
    }

    #endregion

    public event Action OnCharacterAttacks;
    public void CharacterAttacks()
    {
        Debug.Log("A character has attacked");
        OnCharacterAttacks?.Invoke();
    }

    #region Input

    // Relays who hit first?
    private Character _winner;
    public Character PlayerInput(Character c, double time)
    {
        double reactionTime = time - _battleStartTime;
        string formattedReactionTime = $"{reactionTime:F3}";
        Debug.Log("Character " + c.name + " has input in " + formattedReactionTime + " ms.");
        c.DisableControls();
        if (_winner == null) _winner = c;
        return _winner;
    }

    #endregion
}
