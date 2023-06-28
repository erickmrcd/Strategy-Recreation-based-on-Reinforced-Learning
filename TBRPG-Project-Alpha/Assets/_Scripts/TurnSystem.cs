using System;
using UnityEngine;
/// <summary>
/// The turn system.
/// </summary>

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe más de un TurnSystem" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// nexts the turn.
    /// </summary>
    public void nextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Gets the turn number.
    /// </summary>
    /// <returns>An int.</returns>
    public int GetTurnNumber()
    {
        return turnNumber;
    }

    /// <summary>
    /// Are the player turn.
    /// </summary>
    /// <returns>A bool.</returns>
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
