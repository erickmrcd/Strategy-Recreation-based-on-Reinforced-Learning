using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }


    public event EventHandler OnAnyUnitMoveGridPosition;


    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe más de un LevelGrid" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize,
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="unit"></param>
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="unit"></param>
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="lastGridPosition"></param>
    /// <param name="targetGridPosition"></param>
    public void UnitMovedGridpPosition(Unit unit, GridPosition lastGridPosition, GridPosition targetGridPosition)
    {
        RemoveUnitAtGridPosition(lastGridPosition, unit);
        AddUnitAtGridPosition(targetGridPosition, unit);

        OnAnyUnitMoveGridPosition?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetWidht() => gridSystem.GetWidht();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetHeight() => gridSystem.GetHeight();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="centerGridPosition"></param>
    /// <param name="radius"></param>
    /// <param name="isEnemy"></param>
    /// <returns></returns>
    public List<Unit> GetUnitsInRadius(GridPosition centerGridPosition, float radius, bool isEnemy)
    {
        List<Unit> unitsInRadius = new List<Unit>();

        Vector3 centerVector3Position = new Vector3(centerGridPosition.x, 0, centerGridPosition.z);
        // Consideramos todas las posiciones en el grid
        foreach (Unit unit in UnitManager.Instance.GetUnitList())
        {
            GridPosition unitGridPosition = unit.GetGridPosition();
            Vector3 unitVector3Position = new Vector3(unitGridPosition.x, 0, unitGridPosition.z);
            // Calculamos la distancia entre la unidad y el centro de la esfera
            float distance = Vector3.Distance(centerVector3Position, unitVector3Position);

            // Verificamos si la unidad está en el radio y si es enemigo o aliado
            if (distance <= radius && unit.IsEnemy() != isEnemy)
            {
                unitsInRadius.Add(unit);
            }
        }

        return unitsInRadius;
    }

}
