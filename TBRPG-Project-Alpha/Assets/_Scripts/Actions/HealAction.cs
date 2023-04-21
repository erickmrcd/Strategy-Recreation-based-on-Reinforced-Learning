using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : BaseAction
{
    public override string GetActionName()
    {
        return "Healing";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<HealAction>().GetTargetCountAtGridPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        throw new NotImplementedException();
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        throw new NotImplementedException();
    }
}
