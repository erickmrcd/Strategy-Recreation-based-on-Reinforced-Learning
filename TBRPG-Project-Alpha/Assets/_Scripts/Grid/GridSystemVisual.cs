using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        Green,
        Blue,
        Red,
        Yellow,
        RedSoft
    }


    [SerializeField] private Transform gridSystemVisualPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe más de un GridSystemVisual" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidht(),
            LevelGrid.Instance.GetHeight()
        ];

        for (int x = 0; x < LevelGrid.Instance.GetWidht(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransfom =
                    Instantiate(gridSystemVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransfom.GetComponent<GridSystemVisualSingle>();
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += Instance_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMoveGridPosition += Instance_OnAnyUnitMoveGridPosition;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_OnAnyUnitMoveGridPosition(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_OnSelectedActionChanged(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }

    private void Update()
    {
        UpdateGridVisual();
    }
    /// <summary>
    /// 
    /// </summary>
    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidht(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="range"></param>
    /// <param name="gridVisualType"></param>
    private void ShowGridPOsitionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {

        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);


                if (range > 1 && Mathf.Abs(x) <= 1 && Mathf.Abs(z) <= 1)
                {
                    continue;
                }

                if (range > 2)
                {
                    int sum = Mathf.Abs(x) + Mathf.Abs(z);

                    if (sum <= 1 || sum == 2 && (Mathf.Abs(x) == 2 || Mathf.Abs(z) == 2))
                    {
                        continue;
                    }
                }


                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);

                ShowGridPositionList(gridPositionList, gridVisualType);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPositionList"></param>
    /// <param name="gridVisualType"></param>
    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="range"></param>
    /// <param name="gridVisualType"></param>
    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType = GridVisualType.Green;

        switch (selectedAction)
        {
            case MoveAction move:
                gridVisualType = GridVisualType.Green;
                break;
            case SpinAction spin:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shoot:
                gridVisualType = GridVisualType.Red;
                ShowGridPOsitionRange(selectedUnit.GetGridPosition(), shoot.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case MeleeAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case BowAction bowAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPOsitionRange(selectedUnit.GetGridPosition(), bowAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            default:

                break;
        }
        if (selectedAction != null)
            ShowGridPositionList(
                selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridVisualType"></param>
    /// <returns></returns>
    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }

        }
        return null;
    }
}
