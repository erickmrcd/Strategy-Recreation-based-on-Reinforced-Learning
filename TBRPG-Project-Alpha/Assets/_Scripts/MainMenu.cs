using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject settingMenu;
    /// <summary>
    /// Plays the.
    /// </summary>
    /// <param name="name">The name.</param>
    public void Play(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Exits the.
    /// </summary>
    public void Exit()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }

    /// <summary>
    /// Fulls the screen.
    /// </summary>
    /// <param name="isFullScreen">If true, is full screen.</param>
    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    /// <summary>
    /// Sets the volume.
    /// </summary>
    /// <param name="volume">The volume.</param>
    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void closeSetting()
    {
        settingMenu.SetActive(false);
    }
    public void openSetting()
    {
        settingMenu.SetActive(true);
    }
}
