using TMPro;
using UnityEngine;

public class VictoryDefeatUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryDefeatPanel;
    [SerializeField] private TextMeshProUGUI victoryDefeatText;

    /// <summary>
    /// Starts the.
    /// </summary>
    private void Start()
    {
        victoryDefeatPanel.SetActive(false);

        GameManager.Instance.OnPlayerDefeat += Instance_OnPlayerDefeat;
        GameManager.Instance.OnPlayerVictory += Instance_OnPlayerVictory;
    }

    /// <summary>
    /// player victory.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void Instance_OnPlayerVictory(object sender, System.EventArgs e)
    {
        victoryDefeatText.text = "¡Victoria!";
        victoryDefeatPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// player defeat.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void Instance_OnPlayerDefeat(object sender, System.EventArgs e)
    {
        victoryDefeatText.text = "Derrota";
        victoryDefeatPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
