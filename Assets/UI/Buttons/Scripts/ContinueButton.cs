/**
 * File: ContinueButton.cs 
 * Author: Derek Nguyen
 * 
 * Child class for a MenuButton
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MenuButton
{
    /**
     * what happens when the button is pressed
     * 
     * Loads saved level
     */
    public override void ButtonEffects()
    {
        SaveData thing = SaveSystem.LoadSave();
        if(thing == null)
        {
            GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToNextLevel();
        }
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel(thing.room);
    }
}
