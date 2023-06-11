using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BowAction : BaseAction
{
    public event EventHandler<OnBowShootEventArgs> OnBowShoot;
    public event EventHandler OnAiming;

    public class OnBowShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }


    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    [SerializeField] private int maxShootDistance = 1;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int baseWeaponDamage = 0;
    [SerializeField] private int WeaponRangeDamage = 0;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootAnArrow;
    private int numSimulations = 1000;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                if (canShootAnArrow)
                {
                    Aiming();
                    canShootAnArrow = false;
                }
                break;
            case State.Shooting:
                if (!canShootAnArrow)
                {
                    Shoot();
                    canShootAnArrow = true;
                }
                break;
            case State.Cooloff:
                break;
        }

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void Aiming()
    {
        OnAiming?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot()
    {
        if (unit.AttackRoll() <= targetUnit.GetArmorClass())
        {
            OnBowShoot?.Invoke(this, new OnBowShootEventArgs
            {
                targetUnit = targetUnit,
                shootingUnit = unit
            });

            targetUnit.Damage(Random.Range(1, WeaponRangeDamage + 1) + baseWeaponDamage);
        }
        else
        {
            Debug.Log("Miss");
        }

    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.5f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Bow";
    }

    


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return GetValidActionGridPositionList(unitGridPosition);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootAnArrow = true;

        ActionStart(onActionComplete);
    }

    public int GetTargetCountAtGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    private List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (maxShootDistance > 1 && (Mathf.Abs(x) + Mathf.Abs(z) == 1))
                {
                    continue;
                }


                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
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

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        float totalScore = 0f;

        for (int i = 0; i < numSimulations; i++)
        {
            totalScore += SimulateActionScore(new EnemyAIAction { gridPosition = gridPosition });
        }

        float averageScore = totalScore / numSimulations;

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = averageScore
        };
    }

    public override float SimulateActionScore(EnemyAIAction action)
    {
        GridPosition targetGridPosition = action.gridPosition;
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(targetGridPosition);
        if (targetUnit == null)
        {
            return 0f; // No hay un objetivo válido en esta posición
        }

        float score = 0f;

        // Factor de distancia: favorece las posiciones más cercanas al objetivo
        GridPosition unitGridPosition = unit.GetGridPosition();
        int distance = GridPosition.Distance(unitGridPosition, targetGridPosition);
        float distanceFactor = 1f / (1f + distance);
        score += distanceFactor * 100f; // Ponderación de la distancia en la puntuación

        // Factor de salud: favorece los objetivos con menor salud
        float healthFactor = 1f - targetUnit.GetHealthNormalized();
        score += healthFactor * 100f; // Ponderación de la salud en la puntuación

        // Agrega aquí otros factores relevantes para la acción Bow

        return score;
    }

    public override int GetActionPointCost()
    {
        return 2;
    }
}

