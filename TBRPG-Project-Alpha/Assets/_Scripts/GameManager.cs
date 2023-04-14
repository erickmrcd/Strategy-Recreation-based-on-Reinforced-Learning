using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Play,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //Variable
    private List<Unit> playerUnitList;
    private List<Unit> enemyUnitList;
    public event EventHandler OnPlayerVictory;
    public event EventHandler OnPlayerDefeat;

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

    private void Update()
    {

        CheckForVictoryOrDefeat(); 
        
    }

    private void CheckForVictoryOrDefeat()
    {
        if (IAWin())
        {
            OnPlayerDefeat?.Invoke(this,EventArgs.Empty);
        }
        else if (PlayerWin())
        {
            OnPlayerVictory?.Invoke(this,EventArgs.Empty);
        }
    }

    private bool IAWin()
    {
        if (playerUnitList.Count == 0)
        {
            Debug.Log("Has perdido");
            return true;
        }
        return false;
    }

    private bool PlayerWin()
    {
        if (enemyUnitList.Count == 0)
        {
            Debug.Log("Has ganado");
            return true;
        }
        return false;

    }

}
