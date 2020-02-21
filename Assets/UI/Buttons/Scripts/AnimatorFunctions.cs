/**
 * File: AnimatorFunctions.cs 
 * Author: Derek Nguyen
 * 
 * What happens for button Animations
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    //Manages the buttons
    [SerializeField] private ButtonController controller;
    //After running once
    public bool disableOnce;

    /**
     * Plays a sound during a specific frame of the animation
     * 
     * whichSound : audio clip to play
     */ 
    void PlaySound(AudioClip whichSound)
    {
        controller.m_Audio.PlayOneShot(whichSound);
    }
}
