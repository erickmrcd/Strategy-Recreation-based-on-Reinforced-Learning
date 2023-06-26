public class PathNode
{

    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    private bool isWalkable = true;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetGCost()
    {
        return gCost;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetHCost()
    {
        return hCost;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetFCost()
    {
        return fCost;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gCost"></param>
    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hCost"></param>
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    /// <summary>
    /// 
    /// </summary>
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    /// <summary>
    /// 
    /// </summary>
    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pathNode"></param>
    public void SetCameFromPathNode(PathNode pathNode)
    {
        cameFromPathNode = pathNode;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsWalkable()
    {
        return isWalkable;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isWalkable"></param>
    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }


}

