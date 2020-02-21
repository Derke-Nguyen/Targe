/**
 * File: ControlsButton.cs 
 * Author: Derek Nguyen
 * 
 * Child class for a MenuButton
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsButton : MenuButton
{
    // The display that contains all the controls
    public GameObject window;

    /**
     * what happens when the button is pressed
     * 
     * Shows controls game
     */
    public override void ButtonEffects()
    {
        if(!window.activeSelf)
        {
            window.SetActive(true);
        }
    }
}
