using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;


    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();

    }

    /// <summary>
    /// This method returns the action name
    /// Este metodo devuelve el nombre de la acción
    /// </summary>
    /// <returns>String</returns>
    public abstract string GetActionName();
    
    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="onActionComplete"></param>
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract List<GridPosition> GetValidActionGridPositionList();

    /// <summary>
    /// 
    /// Devuelve el coste de la accion
    /// </summary>
    /// <returns>Int</returns>
    public virtual int GetActionPointCost()
    {
        return 1;
    }

    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <param name="onActionComplete"></param>
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);

    }

    /// <summary>
    /// 
    /// </summary>
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Unit GetUnit()
    {
        return unit;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public EnemyAIAction GetBestEnemyAIAction()
    {

        int numSimulationsPerAction = 1000; // Ajusta este valor según la cantidad de simulaciones que deseas realizar por acción
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);

            // Simula la acción varias veces y calcula su resultado promedio
            float totalScore = 0;
            for (int i = 0; i < numSimulationsPerAction; i++)
            {
                float simulationScore = SimulateActionScore(enemyAIAction);
                totalScore += simulationScore;
            }
            enemyAIAction.actionValue = (totalScore / numSimulationsPerAction);

            enemyAIActionList.Add(enemyAIAction);
        }

        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue.CompareTo(a.actionValue));
            return enemyAIActionList[0];
        }
        else
        {
            // No possible Enemy AI Actions
            return null;
        }

    }

    private EnemyAIAction MonteCarloSimulation(List<GridPosition> validGridPositionList)
    {
        int numberOfSimulations = 1000; // El número de simulaciones para cada acción posible
        EnemyAIAction bestAction = null;
        float bestActionScore = float.MinValue;

        foreach (GridPosition gridPosition in validGridPositionList)
        {
            EnemyAIAction action = GetEnemyAIAction(gridPosition);
            float totalScore = 0;

            for (int i = 0; i < numberOfSimulations; i++)
            {
                // Simula el resultado de la acción y obtén una puntuación
                float score = SimulateActionScore(action);
                totalScore += score;
            }

            float averageScore = totalScore / numberOfSimulations;

            if (averageScore > bestActionScore)
            {
                bestActionScore = averageScore;
                bestAction = action;
            }
        }

        return bestAction;
    }

    public abstract float SimulateActionScore(EnemyAIAction action);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
