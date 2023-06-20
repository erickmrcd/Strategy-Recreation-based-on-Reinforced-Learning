using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Random = UnityEngine.Random;
/// <summary>
/// The unit.
/// </summary>

public class Unit : MonoBehaviour
{
    /// <summary>
    /// The character class.
    /// </summary>
    public enum CharacterClass
    {
        Barbarian,
        Bard,
        Cleric,
        Druid,
        Fighter,
        Monk,
        Paladin,
        Ranger,
        Rogue,
        Sorcerer,
        Warlock,
        Wizard
    }
    private const int ACTION_POINTS_MAX = 5;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool isEnemy;
    [SerializeField] private bool isRangedUnit;
    [SerializeField] private String unitName = "Jhon Doe";
    [SerializeField] private int armorClass = 10;
    [SerializeField] private Sprite characterPortrait;
    [SerializeField] private CharacterClass characterClass;
    

    private GridPosition gridPosition;
    private BaseAction[] baseActions;
    private int actionPoints = ACTION_POINTS_MAX;
    private HealthSystem healthSystem;
    private UnitStats unitStats;
    private bool hasMagicAction;

    /// <summary>
    /// Gets or sets a value indicating whether ranged is unit.
    /// </summary>
    public bool IsRangedUnit { get => isRangedUnit; set => isRangedUnit = value; }
    /// <summary>
    /// Gets or sets the character portrait.
    /// </summary>
    public Sprite CharacterPortrait { get => characterPortrait; set => characterPortrait = value; }
    /// <summary>
    /// Gets or sets the unit stats.
    /// </summary>
    public UnitStats UnitStats { get => unitStats; set => unitStats = value; }

    /// <summary>
    /// Awakes the.
    /// </summary>
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActions = GetComponents<BaseAction>();
    }

    /// <summary>
    /// Gets the world position.
    /// </summary>
    /// <returns>A Vector3.</returns>
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    /// <summary>
    /// Gets the unit world position.
    /// </summary>
    /// <returns>A Transform.</returns>
    public Transform GetUnitWorldPosition()
    {
        return transform;
    }

    /// <summary>
    /// Starts the.
    /// </summary>
    private void Start()
    {
        GenerateStats();
        healthSystem.SetHealth(UnitStats.Health);
        SetArmorClass(UnitStats.ArmorModifier);
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += Instance_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

    }

    /// <summary>
    /// Generates the stats.
    /// </summary>
    private void GenerateStats()
    {
        switch (characterClass)
        {
            case CharacterClass.Barbarian:
                UnitStats = new UnitStats { ArmorModifier = 2, DamageModifier = 5, Health = 35 };
                break;
            case CharacterClass.Bard:
                UnitStats = new UnitStats { ArmorModifier = 1, DamageModifier = 3, Health = 22 };
                break;
            case CharacterClass.Cleric:
                UnitStats = new UnitStats { ArmorModifier = 2, DamageModifier = 4, Health = 27 };
                break;
            case CharacterClass.Druid:
                UnitStats = new UnitStats { ArmorModifier = 2, DamageModifier = 4, Health = 25 };
                break;
            case CharacterClass.Fighter:
                UnitStats = new UnitStats { ArmorModifier = 3, DamageModifier = 4, Health = 30 };
                break;
            case CharacterClass.Monk:
                UnitStats = new UnitStats { ArmorModifier = 1, DamageModifier = 4, Health = 25 };
                break;
            case CharacterClass.Paladin:
                UnitStats = new UnitStats { ArmorModifier = 3, DamageModifier = 5, Health = 30 };
                break;
            case CharacterClass.Ranger:
                UnitStats = new UnitStats { ArmorModifier = 2, DamageModifier = 4, Health = 26 };
                break;
            case CharacterClass.Rogue:
                UnitStats = new UnitStats { ArmorModifier = 1, DamageModifier = 5, Health = 24 };
                break;
            case CharacterClass.Sorcerer:
                UnitStats = new UnitStats { ArmorModifier = 1, DamageModifier = 5, Health = 20 };
                break;
            case CharacterClass.Warlock:
                UnitStats = new UnitStats { ArmorModifier = 2, DamageModifier = 5, Health = 22 };
                break;
            case CharacterClass.Wizard:
                UnitStats = new UnitStats { ArmorModifier = 1, DamageModifier = 5, Health = 20 };
                break;
        }
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
    /// Sets the armor class.
    /// </summary>
    /// <param name="armorModifier">The armor modifier.</param>
    public void SetArmorClass(int armorModifier)
    {
        this.armorClass += armorModifier;
    }

    /// <summary>
    /// Gets the current max health.
    /// </summary>
    /// <returns>An int.</returns>
    public int GetCurrentMaxHealth()
    {
        return healthSystem.GetCurrentHealthMax();
    }



}
