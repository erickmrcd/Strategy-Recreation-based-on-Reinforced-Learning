using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="material"></param>
    public void Show(Material material)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = material;
    }
    /// <summary>
    /// 
    /// </summary>
    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
