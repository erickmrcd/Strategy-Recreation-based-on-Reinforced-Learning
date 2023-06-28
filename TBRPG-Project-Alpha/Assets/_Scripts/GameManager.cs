using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The game state.
/// </summary>

public enum GameState
{
    Play,
    GameOver
}
/// <summary>
/// The game manager.
/// </summary>

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Gets the instance.
    /// </summary>
    public static GameManager Instance { get; private set; }

    //Variable
    private List<Unit> playerUnitList;
    private List<Unit> enemyUnitList;

    public event EventHandler OnPlayerVictory;
    public event EventHandler OnPlayerDefeat;
    public event EventHandler OnGamePause;

    /// <summary>
    /// Awakes the.
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe más de un Game Manager" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerUnitList = UnitManager.Instance.GetFriendlyUnitList();
        enemyUnitList = UnitManager.Instance.GetEnemyUnitList();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    /// <summary>
    /// Turns the system_ on turn changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            //OnPlayerTurn?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Updates the.
    /// </summary>
    private void Update()
    {

        CheckForVictoryOrDefeat();
        Pause();
        ChangeTurn();
    }

    /// <summary>
    /// Changes the turn.
    /// </summary>
    private void ChangeTurn()
    {
        if (Input.GetKeyDown(KeyCode.Space) && TurnSystem.Instance.IsPlayerTurn())
        {
            TurnSystem.Instance.nextTurn();
        }
        List<Unit> unitWithoutActionsPoints = new List<Unit>();
        foreach (Unit unit in playerUnitList)
        {
            if (unit.GetActionPoints() < 1)
            {
                unitWithoutActionsPoints.Add(unit);
            }
        }

    
        if ((unitWithoutActionsPoints.Count == playerUnitList.Count) && TurnSystem.Instance.IsPlayerTurn())
        {
            TurnSystem.Instance.nextTurn();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckForVictoryOrDefeat()
    {
        if (IAWin())
        {
            OnPlayerDefeat?.Invoke(this, EventArgs.Empty);
        }
        else if (PlayerWin())
        {
            OnPlayerVictory?.Invoke(this, EventArgs.Empty);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool IAWin()
    {
        if (playerUnitList.Count == 0)
        {
            Debug.Log("Has perdido");
            return true;
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool PlayerWin()
    {
        if (enemyUnitList.Count == 0)
        {
            Debug.Log("Has ganado");
            return true;
        }
        return false;

    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnGamePause?.Invoke(this, EventArgs.Empty);

        };
    }

}
