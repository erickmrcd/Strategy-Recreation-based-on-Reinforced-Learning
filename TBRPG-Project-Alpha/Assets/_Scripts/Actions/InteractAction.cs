using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The interact action.
/// </summary>

public class InteractAction : BaseAction
{

    private int maxInteractDistance = 1;


    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    /// <summary>
    /// Gets the valid action grid position list.
    /// </summary>
    /// <returns>A list of GridPositions.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                //Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);

                //if (door == null)
                //{
                //    // No Door on this GridPosition
                //    continue;
                //}

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
        //Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);

        //door.Interact(OnInteractComplete);

        ActionStart(onActionComplete);
    }

    /// <summary>
    /// Ons the interact complete.
    /// </summary>
    private void OnInteractComplete()
    {
        ActionComplete();
    }

    /// <summary>
    /// Gets the action point cost.
    /// </summary>
    /// <returns>An int.</returns>
    public override int GetActionPointCost()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Simulates the action score.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>A float.</returns>
    public override float SimulateActionScore(EnemyAIAction action)
    {
        throw new NotImplementedException();
    }
}

