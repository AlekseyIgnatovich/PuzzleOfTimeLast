using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoardingManager : MonoBehaviour
{
    public List<GameObject> matchObjects;
    public OnBoardingInstruction specialInstruction;
    public OnBoardingInstruction healingInstruction;

    public void ActivateSpecialIntruction(GameObject _target)
    {
        specialInstruction.target = _target;
        specialInstruction.gameObject.SetActive(true);
    }

    public void ActivateHealingInstruction(GameObject _target)
    {
        GameManager.instance.isFirstSpecial = false;
        _target.transform.SetSiblingIndex(_target.transform.parent.childCount - 1);
        healingInstruction.target = _target;
        healingInstruction.gameObject.SetActive(true);
    }
}
