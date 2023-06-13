using UnityEngine;
using UnityEngine.SceneManagement;

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
}
