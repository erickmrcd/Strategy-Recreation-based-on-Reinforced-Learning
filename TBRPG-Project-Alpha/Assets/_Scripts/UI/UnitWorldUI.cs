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

    private void Start()
    {
        UpdateActionPointstext();
        UpdateHealthBar();
        healthSystem.OnDamage += HealthSystem_OnDamage;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
    }



    private void UpdateActionPointstext()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healtBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
    {
        UpdateActionPointstext();
    }

    private void HealthSystem_OnDamage(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }
}
