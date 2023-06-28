using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The action button UI.
/// </summary>

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private TextMeshProUGUI tooltip;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction baseAction;

    /// <summary>
    /// Sets the base action.
    /// </summary>
    /// <param name="baseAction">The base action.</param>
    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();
        tooltip.text = "Action Points Cost = " +baseAction.GetActionPointCost().ToString();

        button.onClick.AddListener(() =>
        {
            //lamda expresion
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }


    /// <summary>
    /// Updates the selected visual.
    /// </summary>
    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }




}
