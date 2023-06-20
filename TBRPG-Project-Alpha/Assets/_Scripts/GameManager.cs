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

    /// <summary>
    /// Updates the.
    /// </summary>
    private void Update()
    {

        CheckForVictoryOrDefeat();
        Pause();
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

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnGamePause?.Invoke(this, EventArgs.Empty);

        };
    }

}
