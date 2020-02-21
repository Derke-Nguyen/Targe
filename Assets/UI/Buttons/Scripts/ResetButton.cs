/**
 * File: ResetButton.cs 
 * Author: Derek Nguyen
 * 
 * Child class for a MenuButton
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MenuButton
{
    /**
     * what happens when the button is pressed
     * 
     * resets level
     */
    public override void ButtonEffects()
    {
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().Reset();
    }
}
