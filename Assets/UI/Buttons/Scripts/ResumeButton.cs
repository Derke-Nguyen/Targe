using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MenuButton
{
    public override void ButtonEffects()
    {
        GameObject.Find("Pause Menu").SetActive(false);
    }
}
