using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// 
    /// </summary>
    public void nextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetTurnNumber()
    {
        return turnNumber;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
