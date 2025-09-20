using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableSysem : MonoBehaviour
{

    public static EnableDisableSysem instance;

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }

    public List<EnableDisableButtons> listOfButtons = new List<EnableDisableButtons>();
    
    public void EnableAllButtons()
    {
        foreach (var VARIABLE in listOfButtons)
        {
            VARIABLE.EnableButton();
        }
    }

    public void DisableAllButtons()
    {
        foreach (var VARIABLE in listOfButtons)
        {
            VARIABLE.DisableButton();
        }
    }
}
