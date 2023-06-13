using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The fire ball action.
/// </summary>
public class FireBallAction : BaseAction
{

    [SerializeField] private Transform fireProjectilePrefab;

    private int maxThrowDistance = 7;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    /// <summary>
    /// Gets the action name.
    /// </summary>
    /// <returns>A string.</returns>
    public override string GetActionName()
    {
        return "Fire ball";
    }


    /// <summary>
    /// Gets the valid action grid position list.
    /// </summary>
    /// <returns>A list of GridPositions.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;

    }

    /// <summary>
    /// Takes the action.
    /// </summary>
    /// <param name="gridPosition">The grid position.</param>
    /// <param name="onActionComplete">The on action complete.</param>
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform fireBallProjectileTransform = Instantiate(fireProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        FireBall fireBall = fireBallProjectileTransform.GetComponent<FireBall>();
        fireBall.Setup(gridPosition, OnFireBallBehaviourComplete);

        ActionStart(onActionComplete);

    }
    /// <summary>
    /// 
    /// </summary>
    private void OnFireBallBehaviourComplete()
    {
        ActionComplete();
    }

    /// <summary>
    /// Gets the enemy a i action.
    /// </summary>
    /// <param name="gridPosition">The grid position.</param>
    /// <returns>An EnemyAIAction.</returns>
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int numberOfSimulations = 100; // número de simulaciones para realizar
        EnemyAIAction bestAction = null;
        float bestActionScore = float.MinValue;
        List<GridPosition> validActionPositions = GetValidActionGridPositionList();

        foreach (var actionPosition in validActionPositions)
        {
            EnemyAIAction action = new EnemyAIAction
            {
                gridPosition = actionPosition,
                actionValue = 0,
            };

            float totalActionScore = 0;
            for (int i = 0; i < numberOfSimulations; i++)
            {
                totalActionScore += SimulateActionScore(action);
            }

            float averageActionScore = totalActionScore / numberOfSimulations;
            if (averageActionScore > bestActionScore)
            {
                bestActionScore = averageActionScore;
                bestAction = action;
            }
        }

        return bestAction;
    }

    /// <summary>
    /// Simulates the action score.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>A float.</returns>
    public override float SimulateActionScore(EnemyAIAction action)
    {
        float score = 0;
        List<Unit> enemyUnitsInRadius = LevelGrid.Instance.GetUnitsInRadius(action.gridPosition, 4f, isEnemy: true);
        List<Unit> alliedUnitsInRadius = LevelGrid.Instance.GetUnitsInRadius(action.gridPosition, 4f, isEnemy: false);

        // Asumiendo que cada tirada de dado es independiente y uniformemente distribuida, 
        // el valor esperado de una tirada de un dado de 6 caras es 3.5. Como hay 3 tiradas, el daño esperado es 10.5.
        float expectedDamage = 10.5f * enemyUnitsInRadius.Count;

        // Asigna una gran penalización si hay unidades aliadas en el radio.
        float alliedPenalty = alliedUnitsInRadius.Count > 0 ? -1000f : 0;

        // Asigna una penalización si la acción agotaría los puntos de acción del enemigo.
        float actionPointPenalty = (unit.GetActionPoints() - GetActionPointCost() <= 0) ? -10f : 0;

        score = expectedDamage + alliedPenalty;

        return score;
    }

    /// <summary>
    /// Gets the action point cost.
    /// </summary>
    /// <returns>An int.</returns>
    public override int GetActionPointCost()
    {
        return 5;
    }
}
