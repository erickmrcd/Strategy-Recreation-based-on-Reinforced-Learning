using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The move action.
/// </summary>

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float stoppingDistance = 0.001f;
    [SerializeField] private int maxMoveDistance = 4;

    private List<Vector3> positionList;
    private int currentPositionIndex;
    private int numSimulations = 1000;
    private float weightDistance = 1.0f;
    private float weightHealth = 0.5f;

    /// <summary>
    /// Updates the.
    /// </summary>
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);

                ActionComplete();
            }

        }

    }

    /// <summary>
    /// Takes the action.
    /// </summary>
    /// <param name="gridPosition">The grid position.</param>
    /// <param name="onActionComplete">The on action complete.</param>
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }



    /// <summary>
    /// Gets the valid action grid position list.
    /// </summary>
    /// <returns>A list of GridPositions.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!(LevelGrid.Instance.IsValidGridPosition(testGridPosition)))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    //La posicion actual
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //La posicion esta ya ocupada
                    continue;
                }
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }
                int pathfindingDistanceMultiplaer = 10;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplaer)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    /// <summary>
    /// Gets the action name.
    /// </summary>
    /// <returns>A string.</returns>
    public override string GetActionName()
    {
        return "Move";
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
        GridPosition targetPosition = action.gridPosition;
        int distanceToClosestEnemy = GetDistanceToClosestEnemy(targetPosition);
        Unit closestEnemy = GetClosestEnemy(targetPosition);
        float enemyHealthScore = -closestEnemy.GetCurrentHealth();

        if (unit.IsRangedUnit)
        {
            // Si es una unidad de ataque a distancia, es mejor mantener cierta distancia de los enemigos
            int preferredDistance = 2; // Establecer la distancia preferida para unidades de ataque a distancia
            float distanceScore = -Mathf.Abs(preferredDistance - distanceToClosestEnemy);

            // Ponderar las puntuaciones (ajustar los pesos según sea necesario)


            return weightDistance * distanceScore + weightHealth * enemyHealthScore;
        }
        else
        {
            // Si no es una unidad de ataque a distancia, es mejor estar cerca de los enemigos
            float distanceScore = -distanceToClosestEnemy;

            // Ponderar las puntuaciones (ajustar los pesos según sea necesario)
            float weightDistance = 1.0f;
            float weightHealth = 0.2f;

            return weightDistance * distanceScore + weightHealth * enemyHealthScore;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private int GetDistanceToClosestEnemy(GridPosition position)
    {
        List<Unit> allUnits = UnitManager.Instance.GetUnitList();
        int minDistance = int.MaxValue;

        foreach (Unit otherUnit in allUnits)
        {
            if (otherUnit == unit || otherUnit.IsEnemy() == unit.IsEnemy())
            {
                continue; // Ignorar la unidad actual y las unidades del mismo equipo
            }

            int distance = GridPosition.Distance(position, otherUnit.GetGridPosition());
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        return minDistance;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private Unit GetClosestEnemy(GridPosition targetPosition)
    {
        List<Unit> allUnits = UnitManager.Instance.GetUnitList();
        int minDistance = int.MaxValue;
        Unit closestEnemy = null;

        foreach (Unit otherUnit in allUnits)
        {
            if (otherUnit == unit || otherUnit.IsEnemy() == unit.IsEnemy())
            {
                continue; // Ignorar la unidad actual y las unidades del mismo equipo
            }

            int distance = GridPosition.Distance(targetPosition, otherUnit.GetGridPosition());
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = otherUnit;
            }
        }

        return closestEnemy;
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