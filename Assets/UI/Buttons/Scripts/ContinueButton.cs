﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MenuButton
{
    public override void ButtonEffects()
    {
        SaveData thing = SaveSystem.LoadSave();
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel(thing.room);
    }
}
