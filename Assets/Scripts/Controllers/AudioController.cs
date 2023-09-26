using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] backgroundMusic;
    void Start()
    {
        int indexMusicRamdom = Random.Range(0, backgroundMusic.Length);
        AudioClip backgroundMusicSelected = backgroundMusic[indexMusicRamdom];
        audioSource.clip = backgroundMusicSelected;

        audioSource.Play();
    }

    void Update()
    {

    }
}
