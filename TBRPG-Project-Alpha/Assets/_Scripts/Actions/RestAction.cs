using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The rest action.
/// </summary>

public class RestAction : BaseAction
{
    /// <summary>
    /// Gets the action name.
    /// </summary>
    /// <returns>A string.</returns>
    public override string GetActionName()
    {
        return "Rest";
    }

    /// <summary>
    /// Gets the action point cost.
    /// </summary>
    /// <returns>An int.</returns>
    public override int GetActionPointCost()
    {
        return unit.GetActionPoints();
    }

    /// <summary>
    /// Gets the valid action grid position list.
    /// </summary>
    /// <returns>A list of GridPositions.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // La acción de descanso no tiene una lista válida de posiciones, así que devolvemos una lista vacía
        return new List<GridPosition>();
    }

    /// <summary>
    /// Gets the enemy a i action.
    /// </summary>
    /// <param name="gridPosition">The grid position.</param>
    /// <returns>An EnemyAIAction.</returns>
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        // Devolvemos una acción con la posición actual de la unidad y un valor de 0
        // porque no estamos tomando una acción específica sobre el grid
        float totalScore = 0;
        int numSimulations = 1000;
        for (int i = 0; i < numSimulations; i++)
        {
            totalScore += SimulateActionScore(new EnemyAIAction { gridPosition = gridPosition });
        }

        float averageScore = totalScore / numSimulations;

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = averageScore
        };
    }

    /// <summary>
    /// Simulates the action score.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>A float.</returns>
    public override float SimulateActionScore(EnemyAIAction action)
    {
        if (unit.GetActionPoints() == 1)
        {
            Debug.Log(unit.GetActionPoints());
            // Obtenemos la lista de enemigos cercanos
            List<Unit> nearbyEnemies = LevelGrid.Instance.GetUnitsInRadius(unit.GetGridPosition(), 1.0f, true);

            // Si hay al menos un enemigo cercano, aumentamos el valor de la acción de descanso
            if (nearbyEnemies.Count >= 0)
            {
                return 150.0f;  // Puedes ajustar este valor como consideres necesario
            }
        }

        // Si la unidad tiene menos de 3 puntos de acción, devolvemos un valor alto para esta acción
        // Por otro lado, si la unidad tiene 3 o más puntos de acción, devolvemos un valor bajo
        return unit.GetActionPoints() < 3 ? 100.0f : 1.0f;
    }


    /// <summary>
    /// Takes the action.
    /// </summary>
    /// <param name="gridPosition">The grid position.</param>
    /// <param name="onActionComplete">The on action complete.</param>
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        // La acción de descanso simplemente gasta todos los puntos de acción de la unidad
        unit.TrySpendActionPointsToTakeAction(this);
        // Invocamos la acción de finalización
        onActionComplete?.Invoke();
        ActionStart(onActionComplete);
    }
}

