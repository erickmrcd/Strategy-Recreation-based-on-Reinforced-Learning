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

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };

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
        Transform grenadeProjectileTransform = Instantiate(fireProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        FireBall fireBall = grenadeProjectileTransform.GetComponent<FireBall>();
        fireBall.Setup(gridPosition, OnFireBallBehaviourComplete);

        ActionStart(onActionComplete);

    }

    private void OnFireBallBehaviourComplete()
    {
        ActionComplete();
    }

    public override float SimulateActionScore(EnemyAIAction action)
    {
        return 0;
    }
}
