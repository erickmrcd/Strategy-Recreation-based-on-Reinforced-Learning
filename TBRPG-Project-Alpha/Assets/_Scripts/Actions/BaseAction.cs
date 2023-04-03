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
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }
        else
        {
            // No possible Enemy AI Actions
            return null;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);


}
