using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healtBarImage;
    [SerializeField] private HealthSystem healthSystem;
    //[SerializeField] private TextMeshProUGUI unitNameText;

    private void Start()
    {
        UpdateActionPointstext();
        UpdateHealthBar();
        healthSystem.OnDamage += HealthSystem_OnDamage;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        //unitNameText.text = unit.GetUnitName();
    }


    /// <summary>
    /// 
    /// </summary>
    private void UpdateActionPointstext()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateHealthBar()
    {
        healtBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
    {
        UpdateActionPointstext();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HealthSystem_OnDamage(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }
}
