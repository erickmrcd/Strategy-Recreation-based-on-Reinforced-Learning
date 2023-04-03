using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Victory,
    Lose
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //Variable
    private List<Unit> playerUnitList;
    private List<Unit> enemyUnitList;
    private GameState state;
    public static event Action<GameState> OnGameStateChange;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe más de un Game Manager" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
     
        playerUnitList =  UnitManager.Instance.GetFriendlyUnitList();
        enemyUnitList = UnitManager.Instance.GetEnemyUnitList();

        
    }

    public void UpdateGameState(GameState nextState)
    {
        state = nextState;

        switch (nextState)
        {
            case GameState.Victory:
                SetWinCondition();
                break;
            case GameState.Lose:
                SetLoseCondition();
                break;
            default:
                break;
        }

        OnGameStateChange?.Invoke(nextState);
    }

    private void SetLoseCondition()
    {
        if (playerUnitList.Count == 0)
        {
            Debug.Log("Has perdido");
        }
    }

    private void SetWinCondition()
    {
        if (enemyUnitList.Count == 0)
        {
            Debug.Log("Has ganado");
        }
    }

}
