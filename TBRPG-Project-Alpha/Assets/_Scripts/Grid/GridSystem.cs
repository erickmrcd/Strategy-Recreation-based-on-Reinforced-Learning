using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private float cellsize;
    private TGridObject[,] gridObjectsArray;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellsize"></param>
    /// <param name="createGridObject"></param>
    public GridSystem(int width, int height, float cellsize,Func<GridSystem<TGridObject>,GridPosition,TGridObject> createGridObject)
    {
        this.height = height;
        this.width = width;
        this.cellsize = cellsize;


        gridObjectsArray = new TGridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectsArray[x,z] = createGridObject(this, gridPosition);
            }
        } 
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) *cellsize;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellsize),
            Mathf.RoundToInt(worldPosition.z / cellsize));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="debugPrefab"></param>
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform debugTransfom = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransfom.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectsArray[gridPosition.x, gridPosition.z];
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
            gridPosition.z >= 0 &&
            gridPosition.x < width &&
            gridPosition.z < height;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetWidht()
    {
        return width;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetHeight()
    {
        return height;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="copyGridObject"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetGridObject(GridPosition gridPosition, GridObject copyGridObject)
    {
        throw new NotImplementedException();
    }
}
