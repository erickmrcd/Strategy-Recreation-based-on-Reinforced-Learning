using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += Instance_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += Instance_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += Instance_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }


    private void CreateUnitActionButton()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit != null)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActions())
            {
                Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);

                actionButtonUIList.Add(actionButtonUI);
            }
        }

        
    }


    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (selectedUnit != null)
        {
            actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
        }
        
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Instance_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Instance_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Instance_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void Instance_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }


}
