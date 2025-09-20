using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSummonMenuItem : MonoBehaviour
{
    [SerializeField] HeroSummonScreen screen;
    [SerializeField] HeroSummonItemInfo info;

    public void PurchaseHeroSummonItem()
    {
        screen.ShowConfirmationPopup(info);
    }
}
