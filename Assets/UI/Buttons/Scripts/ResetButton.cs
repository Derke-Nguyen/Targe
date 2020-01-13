using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MenuButton
{
    public override void ButtonEffects()
    {
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().Reset();
    }
}
