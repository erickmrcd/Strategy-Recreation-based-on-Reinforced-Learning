using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void SelectScene(string name)
    {
        SceneManager.LoadSceneAsync(name);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Resets the scene.
    /// </summary>
    public void ResetScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
