using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BowAction;
using Random = UnityEngine.Random;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;


    public class OnShootEventArgs : EventArgs
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
    private float totalShootAmount;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

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
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
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

    /// <summary>
    /// 
    /// </summary>
    private void Shoot()
    {
        
        if (unit.AttackRoll() <= targetUnit.GetArmorClass())
        {
            OnShoot?.Invoke(this, new OnShootEventArgs
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
    /// <summary>
    /// 
    /// </summary>
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
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
        return "shoot";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
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

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return GetValidActionGridPositionList(unitGridPosition);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitGridPosition"></param>
    /// <returns></returns>
    private List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
               
                if (maxShootDistance > 1 && ((x == 0 && z == 1) || (x == 0 && z == -1)
                   || (x == 1 && z == 0) || (x == -1 && z == 0)))
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

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Unit GetTargetUnit()
    {
        return targetUnit;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override float SimulateActionScore(EnemyAIAction action)
    {
        return 0;
    }

    public override int GetActionPointCost()
    {
        return 2;
    }
}
