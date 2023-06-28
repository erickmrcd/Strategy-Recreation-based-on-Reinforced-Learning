using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The heal action.
/// </summary>
public class HealAction : BaseAction
{
    public event EventHandler OnHealActionStarted;
    public event EventHandler OnHealActionCompleted;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private int numSimulations = 1000;
    private int maxHealthDistance = 1;

    /// <summary>
    /// The state.
    /// </summary>
    private enum State
    {
        Casting,
        LaunchSpell,
    }

    /// <summary>
    /// Gets the action name.
    /// </summary>
    /// <returns>A string.</returns>
    public override string GetActionName()
    {
        return "Healing";
    }

    /// <summary>
    /// Gets the valid action grid position list.
    /// </summary>
    /// <returns>A list of GridPositions.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxHealthDistance; x <= maxHealthDistance; x++)
        {
            for (int z = -maxHealthDistance; z <= maxHealthDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() != unit.IsEnemy())
                {
                    // Both Units on Different 'team'
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
        state = State.Casting;
        stateTimer = 2f;  // Assumes a 2 second casting time
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        if (targetUnit != null)
        {
            OnHealActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Updates the.
    /// </summary>
    private void Update()
    {
        switch (state)
        {
            case State.Casting:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    state = State.LaunchSpell;
                }
                break;

            case State.LaunchSpell:
                // Assumes a healing value of 25
                //targetUnit.Damage(-25);
                OnHealActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    /// <summary>
    /// Gets the enemy a i action.
    /// </summary>
    /// <param name="gridPosition">The grid position.</param>
    /// <returns>An EnemyAIAction.</returns>
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        List<EnemyAIAction> possibleActions = new List<EnemyAIAction>();

        // Consider all allied units as possible targets for healing
        List<Unit> allies = UnitManager.Instance.GetEnemyUnitList();
        foreach (Unit ally in allies)
        {
            // Add a new AI action for each wounded ally
            if (ally.GetCurrentHealth() < ally.GetCurrentMaxHealth())
            {
                possibleActions.Add(new EnemyAIAction
                {
                    gridPosition = ally.GetGridPosition(),
                    actionValue = 0,
                });
            }
        }

        // Consider self-healing only if the unit is wounded
        Unit self = UnitActionSystem.Instance.GetSelectedUnit();
        if (self.GetCurrentHealth() < self.GetCurrentMaxHealth())
        {
            possibleActions.Add(new EnemyAIAction
            {
                gridPosition = self.GetGridPosition(),
                actionValue = 0,
            });
        }

        // Evaluate each possible action using Monte Carlo simulations
        EnemyAIAction bestAction = null;
        float bestScore = float.MinValue;
        foreach (EnemyAIAction action in possibleActions)
        {
            float score = 0f;
            for (int i = 0; i < numSimulations; i++)
            {
                score += SimulateActionScore(action);
            }
            score /= numSimulations;  // Get average score

            if (score > bestScore)
            {
                bestScore = score;
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
        Unit target = LevelGrid.Instance.GetUnitAtGridPosition(action.gridPosition);
        if (target == null) return float.MinValue;

        // Simulate the healing effect
        float currentHealth = target.GetCurrentHealth();
        float maxHealth = target.GetCurrentMaxHealth();

        // Assume healing restores 25% of max health
        int healedHealth = (int)Math.Min(currentHealth + maxHealth * 0.25f, maxHealth);

        // Return the difference in health as the action score
        return healedHealth - currentHealth;
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
