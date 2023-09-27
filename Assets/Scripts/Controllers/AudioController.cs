using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] backgroundMusic;
    void Start()
    {

    }

    public void Play(MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour == GetComponent<Board>())
        {
            int indexMusicRamdom = Random.Range(0, backgroundMusic.Length);
            AudioClip backgroundMusicSelected = backgroundMusic[indexMusicRamdom];
            audioSource.clip = backgroundMusicSelected;
            audioSource.loop = true;
            audioSource.Play();
        }

        if (monoBehaviour == GetComponent<PieceMovementState>())
        {
            AudioClip backgroundMusicSelected = backgroundMusic[1];
            audioSource.clip = backgroundMusicSelected;
            audioSource.loop = false;
            audioSource.Play();
        }
        if (monoBehaviour == GetComponent<PieceSelectionState>())
        {
            AudioClip backgroundMusicSelected = backgroundMusic[0];
            audioSource.clip = backgroundMusicSelected;
            audioSource.loop = false;
            audioSource.Play();
        }
        if (monoBehaviour == GetComponent<TurnEndState>())
        {
            AudioClip backgroundMusicSelected = backgroundMusic[2];
            audioSource.clip = backgroundMusicSelected;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    void Update()
    {

    }
}
