using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    /// <summary>
    /// Sigleton
    /// </summary>
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe más de un UnitActionSystem" + transform + " - " + Instance);
            Destroy(gameObject);
            return; 
        }

        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }
    /// <summary>
    /// 
    /// </summary>
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if (selectedAction == null) { return; }
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;                
            }
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);


        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    /// <summary>
    /// 
    /// </summary>
    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, unitLayerMask))
            {
                if (hitInfo.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (selectedUnit == unit)
                    {
                        return false;
                    }

                    if (unit.IsEnemy())
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }

            
        }
        return false;


    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public  BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit"></param>
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        if (selectedUnit != null)
        {
            SetSelectedAction(unit.GetAction<MoveAction>());
            OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        }
        
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseAction"></param>
    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
