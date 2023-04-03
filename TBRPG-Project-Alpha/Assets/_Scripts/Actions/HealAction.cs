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
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
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
