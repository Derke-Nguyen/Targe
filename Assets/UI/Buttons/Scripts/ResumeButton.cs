/**
 * File: ResumeButton.cs 
 * Author: Derek Nguyen
 * 
 * Child class for a MenuButton
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MenuButton
{
    /**
     * what happens when the button is pressed
     * 
     * unpauses game
     */
    public override void ButtonEffects()
    {
        GameObject.Find("GUI").GetComponent<GUIManager>().Resume();
    }
}
