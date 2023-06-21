using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// The scene selector.
/// </summary>

public class SceneSelector : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void SelectScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Resets the scene.
    /// </summary>
    public void ResetScene()
    {
          SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
