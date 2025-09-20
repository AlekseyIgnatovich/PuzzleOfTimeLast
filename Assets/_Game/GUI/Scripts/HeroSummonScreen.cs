using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSummonScreen : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] HeroBase heroBase;
    [SerializeField] GameObject popup;
    [SerializeField] GameObject buyPopup;

    HeroSummonItemInfo currentHeroSelected;
    public void OpenHeroUnlockScreen()
    {
        gameManager.SetState(GameState.HeroUnlock);
        currentHeroSelected = null;
        gameObject.SetActive(false);
    }

    public void ShowPopup()
    {
        popup.SetActive(true);
    }
    public void ShowConfirmationPopup(HeroSummonItemInfo _summonSelected)
    {
        currentHeroSelected = _summonSelected;
        buyPopup.SetActive(true);
    }

    public void ConfirmPurchase()
    {
        buyPopup.SetActive(false);
        if (heroBase.diamonds - currentHeroSelected.priceAmount >= 0)
        {
            Debug.Log("Hero Summon Card purchased");
            heroBase.ModifyDiamonds(-currentHeroSelected.priceAmount);
            GameManager.instance.rewardManager.itemsData.AddItem(currentHeroSelected.heroSummon);
            ShowPopup();

        }
        else
        {
            gameManager.errorPopup.SetActive(true);
        }
    }
}

[System.Serializable]
public class HeroSummonItemInfo
{
    public ItemObject heroSummon;
    public float priceAmount;

}
