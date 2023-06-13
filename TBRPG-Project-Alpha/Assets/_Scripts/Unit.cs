using System;
using UnityEngine;
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

    /// <summary>
    /// Gets or sets a value indicating whether ranged is unit.
    /// </summary>
    public bool IsRangedUnit { get => isRangedUnit; set => isRangedUnit = value; }
    /// <summary>
    /// Gets or sets the character portrait.
    /// </summary>
    public Sprite CharacterPortrait { get => characterPortrait; set => characterPortrait = value; }

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActions = GetComponents<BaseAction>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetUnitName()
    {
        return unitName;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetActionPoints()
    {
        return actionPoints;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool IsEnemy()
    {
        return isEnemy;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int AttackRoll()
    {
        return Random.Range(1, 21);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="damageAmount"></param>
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHealth()
    {
        return healthSystem.GetCurrentHealth();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetArmorClass()
    {
        return armorClass;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetCurrentMaxHealth()
    {
        return healthSystem.GetCurrentHealthMax();
    }



}
