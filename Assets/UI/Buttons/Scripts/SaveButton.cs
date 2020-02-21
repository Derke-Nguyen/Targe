/**
 * File: SaveButton.cs 
 * Author: Derek Nguyen
 * 
 * Child class for a MenuButton
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MenuButton
{
    /**
     * what happens when the button is pressed
     * 
     * saves level
     */
    public override void ButtonEffects()
    {
        SaveSystem.SaveLevel(GameObject.Find("LevelManager").GetComponent<LevelManager>());
    }
}
