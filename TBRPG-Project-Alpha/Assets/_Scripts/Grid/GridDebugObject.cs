using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;


    private object gridObject;

    /// <summary>
    /// Sets the grid object.
    /// </summary>
    /// <param name="gridObject">The grid object.</param>
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    /// <summary>
    /// Updates the.
    /// </summary>
    protected virtual void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }
}
