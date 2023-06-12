using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override string GetActionName()
    {
        return "Fire ball";
    }


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
        float actionPointPenalty = (unit.GetActionPoints() - GetActionPointCost() <= 0) ? -500f : 0;

        score = expectedDamage + alliedPenalty + actionPointPenalty;

        return score;
    }



    public override int GetActionPointCost()
    {
        return 5;
    }
}
