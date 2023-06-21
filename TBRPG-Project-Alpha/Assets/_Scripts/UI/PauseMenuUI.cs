using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    // Start is called before the first frame update
    /// <summary>
    /// Starts the.
    /// </summary>
    void Start()
    {
        pauseMenuPanel.SetActive(false);
        GameManager.Instance.OnGamePause += Instance_OnGamePause;
    }

    /// <summary>
    /// Instance_S the on game pause.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void Instance_OnGamePause(object sender, System.EventArgs e)
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
