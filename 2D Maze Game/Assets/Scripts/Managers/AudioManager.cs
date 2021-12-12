using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioSource audioSource;

    private static AudioManager instance = null;
    public bool isPlaying { get; set; } = true;
    public static AudioManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Play()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    public void Stop()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}    

