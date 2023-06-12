using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        SceneManager.LoadScene(name);
    }
    /// <summary>
    /// 
    /// </summary>
    public void Exit()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }
}
