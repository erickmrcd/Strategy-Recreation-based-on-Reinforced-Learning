using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base action.
/// </summary>
public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;


    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;


    /// <summary>
    /// Awakes the.
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
    /// Metodo implementado en cada accion que se llama cuando el jugador ejecuta una accion
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="onActionComplete"></param>
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    /// <summary>
    /// 
    /// Devuelve un verdadero o falso si la casilla se puede utilizar
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns>True o falso dependiendo si la casilla es valida</returns>
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    /// <summary>
    /// Obtiene una lista de las casillas que son validas para ejecutar la acción
    /// </summary>
    /// <returns>Devuelve una lista con las posiciones que son validas</returns>
    public abstract List<GridPosition> GetValidActionGridPositionList();

    /// <summary>
    /// Devuelve el coste de la accion
    /// </summary>
    /// <returns>Int</returns>
    public abstract int GetActionPointCost();

    /// <summary>
    /// Metodo que se encarga de llamar al evento que inicia una acción
    /// </summary>
    /// <param name="onActionComplete"></param>
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);

    }

    /// <summary>
    /// Metodo que se encarga de avisar que se ha terminado la acción
    /// </summary>
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Obtiene el componente unidad que contiene esta acción
    /// </summary>
    /// <returns>Devuelve la unidad seleccionada que contiene las acciones</returns>
    public Unit GetUnit()
    {
        return unit;
    }

    /// <summary>
    /// Metodo que se encarga de obtener las acciones a realizar por la IA contrincante usando el algoritmo de montercarlo
    /// </summary>
    /// <returns>Acción a ejecutar por la IA</returns>
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

    /// <summary>
    /// Metodo abstracto para simular los diferentes resultados en una acción
    /// </summary>
    /// <param name="action"></param>
    /// <returns>El peso de la acción a ejecutar</returns>
    public abstract float SimulateActionScore(EnemyAIAction action);

    /// <summary>
    /// Metodo que obtiene la acción de la IA
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns>Devuelve la acción con un objetivo y un peso especifico</returns>
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
