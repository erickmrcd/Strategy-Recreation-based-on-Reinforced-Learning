using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void Exit()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }
}
