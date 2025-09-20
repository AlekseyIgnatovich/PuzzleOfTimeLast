using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableButtonsOnEnable : MonoBehaviour
{

    

    private void OnEnable()
    {
        EnableDisableSysem.instance.DisableAllButtons();
    }

    private void OnDisable()
    {
        EnableDisableSysem.instance.EnableAllButtons();
    }
}
