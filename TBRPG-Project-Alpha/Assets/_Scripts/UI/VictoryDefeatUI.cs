using TMPro;
using UnityEngine;

public class VictoryDefeatUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryDefeatPanel;
    [SerializeField] private TextMeshProUGUI victoryDefeatText;

    private void Start()
    {
        victoryDefeatPanel.SetActive(false);

        GameManager.Instance.OnPlayerDefeat += Instance_OnPlayerDefeat;
        GameManager.Instance.OnPlayerVictory += Instance_OnPlayerVictory;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_OnPlayerVictory(object sender, System.EventArgs e)
    {
        victoryDefeatText.text = "¡Victoria!";
        victoryDefeatPanel.SetActive(true);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_OnPlayerDefeat(object sender, System.EventArgs e)
    {
        victoryDefeatText.text = "Derrota";
        victoryDefeatPanel.SetActive(true);
    }
}
