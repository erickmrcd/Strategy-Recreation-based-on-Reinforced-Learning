using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

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
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }
                Unit unitTarget = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                Unit unit = GetUnit();
                if(unitTarget.IsEnemy() == unit.IsEnemy())
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
        float totalScore = 0f;
        int numSimulations = 1000;
        for (int i = 0; i < numSimulations; i++)
        {
            totalScore += SimulateActionScore(new EnemyAIAction { gridPosition = gridPosition });
        }

        float averageScore = totalScore / numSimulations;
        Debug.Log("Fireball score: " + averageScore);
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
        GridPosition targetGridPosition = action.gridPosition;
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(targetGridPosition);
        if (targetUnit == null)
        {
            return -1000f; // No hay un objetivo válido en esta posición
        }

        float score = 0f;

        // Factor de distancia: favorece las posiciones más cercanas al objetivo
        GridPosition unitGridPosition = unit.GetGridPosition();
        int distance = GridPosition.Distance(unitGridPosition, targetGridPosition);
        score += distance <= maxThrowDistance-2 ? -100f : (1f / (1f + distance))*100f; // Penalización por estar demasiado cerca

        // Factor de salud: favorece los objetivos con menor salud
        float healthFactor = 1f - targetUnit.GetHealthNormalized();
        score += healthFactor * 100f; // Ponderación de la salud en la puntuación
        
        return score;
    }

    /// <summary>
    /// Gets the action point cost.
    /// </summary>
    /// <returns>An int.</returns>
    public override int GetActionPointCost()
    {
        return 3;
    }
}
