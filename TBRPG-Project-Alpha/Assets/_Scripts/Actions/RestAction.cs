using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAction : BaseAction
{
    private Unit targetUnit;

    public override string GetActionName()
    {
        return "Rest";
    }

    public override int GetActionPointCost()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        return selectedUnit.GetActionPoints();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // La acción de descanso no tiene una lista válida de posiciones, así que devolvemos una lista vacía
        return new List<GridPosition>();
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        // Devolvemos una acción con la posición actual de la unidad y un valor de 0
        // porque no estamos tomando una acción específica sobre el grid
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override float SimulateActionScore(EnemyAIAction action)
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit.GetActionPoints() == 1)
        {
            // Obtenemos la lista de enemigos cercanos
            List<Unit> nearbyEnemies = LevelGrid.Instance.GetUnitsInRadius(selectedUnit.GetGridPosition(), 1.0f, true);

            // Si hay al menos un enemigo cercano, aumentamos el valor de la acción de descanso
            if (nearbyEnemies.Count > 0)
            {
                return 150.0f;  // Puedes ajustar este valor como consideres necesario
            }
        }

        // Si la unidad tiene menos de 3 puntos de acción, devolvemos un valor alto para esta acción
        // Por otro lado, si la unidad tiene 3 o más puntos de acción, devolvemos un valor bajo
        return selectedUnit.GetActionPoints() < 3 ? 100.0f : 1.0f;
    }


    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        // La acción de descanso simplemente gasta todos los puntos de acción de la unidad
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        selectedUnit.TrySpendActionPointsToTakeAction(this);

        // Invocamos la acción de finalización
        onActionComplete?.Invoke();
    }
}

