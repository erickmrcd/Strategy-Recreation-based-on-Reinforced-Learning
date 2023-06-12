using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridSystem"></param>
    /// <param name="gridPosition"></param>
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }
    
    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }
}
