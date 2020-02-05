using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsButton : MenuButton
{
    public GameObject window;

    public override void ButtonEffects()
    {
        window.SetActive(true);
    }
}
