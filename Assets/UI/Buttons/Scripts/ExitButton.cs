/**
 * File: ExitButton.cs 
 * Author: Derek Nguyen
 * 
 * Child class for a MenuButton
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MenuButton
{
    /**
     * what happens when the button is pressed
     * 
     * Quits game
     */
    public override void ButtonEffects()
    {
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().ToQuit();
    }
}
