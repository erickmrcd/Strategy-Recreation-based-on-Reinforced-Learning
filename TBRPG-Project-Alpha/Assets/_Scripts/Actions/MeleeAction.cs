using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// The melee action.
/// </summary>

public class MeleeAction : BaseAction
{

    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int baseWeaponDamage;
    [SerializeField] private int WeaponRangeDamage;


    /// <summary>
    /// The state.
    /// </summary>
    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private int maxSwordDistance = 1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private int numSimulations = 1000;

    /// <summary>
    /// Updates the.
    /// </summary>
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:


                if ((unit.AttackRoll() + unit.UnitStats.DamageModifier) <= targetUnit.GetArmorClass())
                {
                    state = State.SwingingSwordAfterHit;
                    float afterHitStateTime = 0.5f;
                    stateTimer = afterHitStateTime;
                    targetUnit.Damage((Random.Range(1, WeaponRangeDamage + 1) + baseWeaponDamage));
                    OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Debug.Log("Miss");
                }

                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }


    /// <summary>
    /// Gets the action name.
    /// </summary>
    /// <returns>A string.</returns>
    public override string GetActionName()
    {
        return "Melee";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public int GetTargetCountAtGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitGridPosition"></param>
    /// <returns></returns>
    private List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxSwordDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on same 'team'
                    continue;
                }
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition());
                float unitShoulderHeight = 1.7f;

                if (Physics.Raycast(
                    unit.GetWorldPosition() + Vector3.up * unitShoulderHeight,
                    shootDir,
                    Vector3.Distance(unit.GetWorldPosition(), targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                {
                    continue;
                }



                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    /// <summary>
    /// Gets the valid action grid position list.
    /// </summary>
    /// <returns>A list of GridPositions.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
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

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on same 'team'
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
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetMaxSwordDistance()
    {
        return maxSwordDistance;
    }

    /// <summary>
    /// Gets the enemy a i action.
    /// </summary>
    /// <param name="gridPosition">The grid position.</param>
    /// <returns>An EnemyAIAction.</returns>
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        float totalScore = 0f;

        for (int i = 0; i < numSimulations; i++)
        {
            totalScore += SimulateActionScore(new EnemyAIAction { gridPosition = gridPosition });
        }

        float averageScore = totalScore / numSimulations;
        Debug.Log("melee score: " + averageScore);
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
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(action.gridPosition);

        // Asignar una puntuación más alta si el objetivo tiene menos vida
        float healthScore = -targetUnit.GetHealthNormalized();

        // Asignar una puntuación más alta si el arma causa más daño
        float damageScore = Random.Range(1, WeaponRangeDamage + 1) + baseWeaponDamage;

        // Ponderar las puntuaciones (ajustar los pesos según sea necesario)

        float weightHealth = 1.0f;
        float weightDamage = 0.5f;

        float totalScore = weightHealth * healthScore + weightDamage * damageScore;

        return totalScore;
    }

    /// <summary>
    /// Gets the action point cost.
    /// </summary>
    /// <returns>An int.</returns>
    public override int GetActionPointCost()
    {
        return 2;
    }
}
