using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyItemPopup : MonoBehaviour
{

    [SerializeField] HeroBase heroBase;
    [SerializeField] ItemsData itemsData;
    [SerializeField] LotteryRewardPanel lotteryPanel;
    [SerializeField] ShopHeroReward heroRewardPanel;
    [Space]
    ShopItem item;
    [Space]
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textDescription;

    public void Setup(ShopItem _item)
    {
        if (_item == null) { return; }

        gameObject.SetActive(true);
        item = _item;

        ShopPlayItem spi = _item as ShopPlayItem;
        if (spi != null)
        {
            textName.text = $"{spi.item.item.Name}";

            switch (itemsData.selectedCurrency)
            {
                case Currency.Coins:
                    textDescription.text = $"Buy a {textName.text} for {spi.item.item.coinsCost} Coins?";
                    break;
                case Currency.Diamonds:
                    textDescription.text = $"Buy a {textName.text} for {spi.item.item.diamondsCost} Diamonds?";
                    break;
            }
            return;
        }

        ShopHeroItem shi = _item as ShopHeroItem;
        if (shi != null)
        {
            textName.text = $"{shi.Name}";
            itemsData.SwitchCurrency(1);

            switch (itemsData.selectedCurrency)
            {
                case Currency.Coins:
                    textDescription.text = $"Buy a {shi.Name} for {shi.costCoins} Coins?";
                    break;
                case Currency.Diamonds:
                    textDescription.text = $"Buy a {shi.Name} for {shi.costDiamonds} Diamonds?";
                    break;
            }
            return;
        }

        ShopLotteryItem sli = _item as ShopLotteryItem;
        if (sli != null)
        {
            textName.text = $"{sli.item.Name}";
            textDescription.text = $"{sli.item.description}";
            return;
        }
    }

    public void BuyItem()
    {
        ShopPlayItem spi;
        ShopHeroItem shi;
        ShopLotteryItem sli;

        switch (itemsData.selectedCurrency)
        {
            case Currency.Coins:
                spi = item as ShopPlayItem;
                if (spi != null)
                {
                    if (heroBase.coins >= spi.item.item.coinsCost)
                    {
                        print($"bought an item with coins");
                        heroBase.ModifyCoins(-spi.item.item.coinsCost);
                        itemsData.AddItem(spi.item);
                    }
                    else
                    {
                        GameManager.instance.errorPopup.SetActive(true);
                    }
                    break;
                }
                shi = item as ShopHeroItem;
                if (shi != null)
                {
                    if (heroBase.coins >= shi.costCoins)
                    {
                        print($"bought a hero with coins");
                        heroBase.ModifyCoins(-shi.costCoins);
                        HeroCard card = shi.GetCard();
                        GameManager.instance.ShowHeroCardAnimation(card);
                        heroBase.AddCardToInventory(card);
                        heroRewardPanel.Setup(card);
                    }
                    else
                    {
                        GameManager.instance.errorPopup.SetActive(true);
                    }
                    break;
                }
                sli = item as ShopLotteryItem;
                if (sli != null)
                {
                    if (heroBase.coins >= sli.item.coinsCost)
                    {
                        print($"bought a lottery with coins");
                        heroBase.ModifyCoins(-sli.item.coinsCost);
                        lotteryPanel.Open(sli.item);
                    }
                    else
                    {
                        GameManager.instance.errorPopup.SetActive(true);
                    }
                    break;
                }
                break;
            case Currency.Diamonds:
                spi = item as ShopPlayItem;
                if (spi != null)
                {
                    if (heroBase.diamonds >= spi.item.item.diamondsCost)
                    {
                        print($"bought an item with coins");
                        heroBase.ModifyDiamonds(-spi.item.item.diamondsCost);
                        itemsData.AddItem(spi.item);
                    }
                    else
                    {
                        GameManager.instance.errorPopup.SetActive(true);
                    }
                    break;
                }
                shi = item as ShopHeroItem;
                if (shi != null)
                {
                    if (heroBase.diamonds >= shi.costDiamonds)
                    {
                        print($"bought a hero with coins");
                        heroBase.ModifyDiamonds(-shi.costDiamonds);
                        HeroCard card = shi.GetCard();
                        GameManager.instance.ShowHeroCardAnimation(card);
                        heroBase.AddCardToInventory(card);
                        heroRewardPanel.Setup(card);
                    }
                    else
                    {
                        GameManager.instance.errorPopup.SetActive(true);
                    }
                    break;
                }
                sli = item as ShopLotteryItem;
                if (sli != null)
                {
                    if (heroBase.diamonds >= sli.item.diamondsCost)
                    {
                        print($"bought a lottery with coins");
                        heroBase.ModifyDiamonds(-sli.item.diamondsCost);
                        lotteryPanel.Open(sli.item);
                    }
                    else
                    {
                        GameManager.instance.errorPopup.SetActive(true);
                    }
                    break;
                }
                break;
        }

        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
