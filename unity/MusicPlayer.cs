using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip music;

    void Start()
    {
        AudioManager.PlayMusic(music);
    }

}
