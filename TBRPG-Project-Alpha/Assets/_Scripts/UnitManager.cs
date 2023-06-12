using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    public static UnitManager Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;

    private void Awake()
    {

        if (Instance != null)
        {
            Debug.LogError("Existe m�s de un UnitManager" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Unit_OnAnyUnitSpawned(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit);

        }
        else
        {
            friendlyUnitList.Add(unit);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Unit_OnAnyUnitDead(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
}

