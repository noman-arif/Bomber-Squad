using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource playAudio;
    public AudioClip blastAudio;
    public AudioClip throwSound;
    public AudioClip winSound;
    public AudioClip gameOverSound;
    public AudioClip collectSound;
    // Start is called before the first frame update
    void Start()
    {
        playAudio = GetComponent<AudioSource>();
    }
}
