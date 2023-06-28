using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnBtn;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnVisualGameObject;


    private void Start()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.nextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += Instance_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();



    }

    /// <summary>
    /// Instance_S the on turn changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void Instance_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    /// <summary>
    /// Updates the turn text.
    /// </summary>
    private void UpdateTurnText()
    {
        turnNumberText.text = "Turno: " + TurnSystem.Instance.GetTurnNumber();
    }

    /// <summary>
    /// Updates the enemy turn visual.
    /// </summary>
    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    /// <summary>
    /// Updates the end turn button visibility.
    /// </summary>
    private void UpdateEndTurnButtonVisibility()
    {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
