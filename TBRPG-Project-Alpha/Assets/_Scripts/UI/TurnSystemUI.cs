using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnBtn;
    [SerializeField] private TextMeshProUGUI turnNumberText;


    private void Start()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.nextTurn();
        });
        UpdateTurnText();

        TurnSystem.Instance.OnTurnChanged += Instance_OnTurnChanged;

    }

    private void Instance_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = "Turno: " + TurnSystem.Instance.GetTurnNumber() ;
    }
}
