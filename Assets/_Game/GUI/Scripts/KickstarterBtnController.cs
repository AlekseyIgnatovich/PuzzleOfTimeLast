using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickstarterBtnController : MonoBehaviour
{
    [SerializeField] GameObject titleTxt;
    [SerializeField] GameObject mssgBtn;
    [SerializeField] GameObject mssgWindow;

    private void Start()
    {
        ToggleActivateKSMessage(true);
    }

    public void ToggleActivateKSMessage(bool _activate)
    {
//        Debug.Log("Activating Kickstarter button: " + _activate);
        mssgBtn.SetActive(_activate);
        titleTxt.SetActive(!_activate);
    }


    //This function is called from the CollectRewardBtn in the Kickstarter message pop up
    public void OpenRewardLink()
    {
        Application.OpenURL("https://www.kickstarter.com/projects/knoxdigital/puzzle-of-time-a-new-mobile-puzzle-rpg");
    }
}
