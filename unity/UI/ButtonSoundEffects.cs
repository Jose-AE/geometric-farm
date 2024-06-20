using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffects : MonoBehaviour
{
    [SerializeField] AudioClip sfx;


    public void PlaySound()
    {
        AudioManager.PlaySFX(sfx);
    }

}
