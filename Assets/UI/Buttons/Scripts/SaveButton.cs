using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MenuButton
{
    public override void ButtonEffects()
    {
        SaveSystem.SaveLevel(GameObject.Find("LevelManager").GetComponent<LevelManager>());
    }
}
