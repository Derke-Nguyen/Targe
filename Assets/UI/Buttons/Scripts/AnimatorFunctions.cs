using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] private MenuButtonController controller;
    public bool disableOnce;

    void PlaySound(AudioClip whichSound)
    {
        controller.audio.PlayOneShot(whichSound);
    }
}
