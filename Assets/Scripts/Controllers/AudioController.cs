using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clipList;
    void Start()
    {

    }

    public void Play(MonoBehaviour monoBehaviour)
    {
        if (monoBehaviour == GetComponent<Board>())
        {
            int indexMusicRamdom = Random.Range(0, clipList.Length);
            AudioClip audioSelected = clipList[indexMusicRamdom];
            audioSource.clip = audioSelected;
            audioSource.loop = true;
            audioSource.Play();
        }

        if (monoBehaviour == GetComponent<PieceMovementState>())
        {
            AudioClip audioSelected = clipList[1];
            audioSource.clip = audioSelected;
            audioSource.loop = false;
            audioSource.Play();
        }
        if (monoBehaviour == GetComponent<PieceSelectionState>())
        {
            AudioClip audioSelected = clipList[0];
            audioSource.clip = audioSelected;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayLoseSoundGame()
    {
        audioSource.Stop();
        AudioClip audioSelected = clipList[3];
        audioSource.clip = audioSelected;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayWinSoundGame()
    {
        audioSource.Stop();
        AudioClip audioSelected = clipList[2];
        audioSource.clip = audioSelected;
        audioSource.loop = false;
        audioSource.Play();
    }
    void Update()
    {

    }
}
