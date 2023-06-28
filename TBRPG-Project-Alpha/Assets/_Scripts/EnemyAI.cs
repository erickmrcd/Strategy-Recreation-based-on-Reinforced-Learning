using System;
using UnityEngine;

/// <summary>
/// The enemy a i.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// The state.
    /// </summary>
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private State state;
    private float timer;

    /// <summary>
    /// Awakes the.
    /// </summary>
    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    /// <summary>
    /// Starts the.
    /// </summary>
    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += Instance_OnTurnChanged;
    }

    /// <summary>
    /// Updates the.
    /// </summary>
    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {

                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        //No more enemy can take actions
                        TurnSystem.Instance.nextTurn();
                    }

                }
                break;
            case State.Busy:
                break;
        }


    }

    /// <summary>
    /// Sets the state taking turn.
    /// </summary>
    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    /// <summary>
    /// Instance_S the on turn changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void Instance_OnTurnChanged(object sender, System.EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }

    }

    /// <summary>
    /// Tries the take enemy a i action.
    /// </summary>
    /// <param name="onEnemyAIActionComplete">The on enemy a i action complete.</param>
    /// <returns>A bool.</returns>
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {

        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
            
        }
        return false;
    }

    /// <summary>
    /// Tries the take enemy a i action.
    /// </summary>
    /// <param name="enemyUnit">The enemy unit.</param>
    /// <param name="onEnemyAIActionComplete">The on enemy a i action complete.</param>
    /// <returns>A bool.</returns>
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        UnitActionSystem.Instance.SetSelectedUnit(enemyUnit);
        foreach (BaseAction baseAction in enemyUnit.GetBaseActions())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                // No puede realizar la acción
                continue;
            }

            // Utiliza el algoritmo de Monte Carlo implementado en GetBestEnemyAIAction()
            // para evaluar esta acción
            EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();

            if (bestEnemyAIAction == null || (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue))
            {
                bestEnemyAIAction = testEnemyAIAction;
                bestBaseAction = baseAction;
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }
}
