using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    /// <summary>
    /// Sets the master volume.
    /// </summary>
    /// <param name="volume">The volume.</param>
    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("Volume", volume);
    }
}
