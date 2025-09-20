using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableDisableButtons : MonoBehaviour
{

    private Button thisBtn;

    private void Awake()
    {
        thisBtn = GetComponent<Button>();
    }

    public void EnableButton()
    {
        thisBtn.enabled = true;
    }

    public void DisableButton()
    {
        thisBtn.enabled = false;
    }
}
