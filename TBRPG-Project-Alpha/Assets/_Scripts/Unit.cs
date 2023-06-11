using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 5;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;
    [SerializeField] private bool isRangedUnit;
    [SerializeField] private String unitName = "Jhon Doe";
    [SerializeField] private int armorClass = 10;
    [SerializeField] private Sprite characterPortrait;

    private GridPosition gridPosition;
    private BaseAction[] baseActions;
    private int actionPoints = ACTION_POINTS_MAX;
    private HealthSystem healthSystem;

    public bool IsRangedUnit { get => isRangedUnit; set => isRangedUnit = value; }
    public Sprite CharacterPortrait { get => characterPortrait; set => characterPortrait = value; }

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActions = GetComponents<BaseAction>();
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += Instance_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

    }

    

    public void Damage()
    {
        Debug.Log(transform + " damage!");
    }

    private void Update()
    {
        

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridpPosition(this, oldGridPosition, newGridPosition);

        }



    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActions)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActions()
    {
        return baseActions;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseAction"></param>
    /// <returns></returns>
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Se puede cambiar más adelante
    /// </summary>
    /// <param name="baseAction"></param>
    /// <returns></returns>
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {

        if (actionPoints >= baseAction.GetActionPointCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }


    private void Instance_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
        
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public string GetUnitName()
    {
        return unitName;
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public int AttackRoll()
    {
        return Random.Range(1, 21);
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    public int GetCurrentHealth()
    {
        return healthSystem.GetCurrentHealth();
    }

    public int GetArmorClass()
    {
        return armorClass;
    }
    public int GetCurrentMaxHealth()
    {
        return healthSystem.GetCurrentHealthMax();
    }

    

}
