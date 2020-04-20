using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Used to play an audio clip
    public void PlayAudio(AudioClip newClip)
    {
        audioSource.PlayOneShot(newClip, 0.5f);
    }
}
