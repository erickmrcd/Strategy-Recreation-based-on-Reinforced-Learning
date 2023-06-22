using UnityEngine;

public class MouseWorld : MonoBehaviour
{


    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, instance.mousePlaneLayerMask);
        return hitInfo.point;
    }
}
