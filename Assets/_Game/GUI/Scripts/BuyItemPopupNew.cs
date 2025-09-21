using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;

public class BuyItemPopupNew : MonoBehaviour
{

    [SerializeField] HeroBase heroBase;
    [SerializeField] ItemsData itemsData;
    [Space]
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] CodelessIAPButton iapButton;

    ItemsPackPanel itemsPack;
    PackItem item;

    public void Setup(ItemsPackPanel _itemPack, PackItem _item)
    {
        item = _item;
        if (_itemPack != null)
        {
            iapButton.productId = _itemPack.Name;
            itemsPack = _itemPack;
        }

        gameObject.SetActive(true);

        label.text = "";
        float _cost = 0;
        string _currency = Currency.Cash.ToString();

        if (item != null)
        {
            label.text = item.GetName();
            _cost = item.cost;
            _currency = item.currency.ToString();
        }
        if (itemsPack != null)
        {
            label.text = itemsPack.Name;
            _cost = itemsPack.cost;
            _currency = itemsPack.currency.ToString();
        }

        description.text = $"Buy {label.text} for {_cost} {_currency}";
    }
    public void BuyItem()
    {

        if (item != null)
        {
            bool _canbuy = false;
            switch (item.currency)
            {
                case Currency.Coins: _canbuy = item.cost <= heroBase.coins; break;
                case Currency.Diamonds: _canbuy = item.cost <= heroBase.diamonds; break;
                case Currency.Cash:
                case Currency.Video:
                    _canbuy = true; break;
            }
            if (_canbuy)
            {
                switch (item.currency)
                {
                    case Currency.Coins: heroBase.ModifyCoins(-item.cost); break;
                    case Currency.Diamonds: heroBase.ModifyDiamonds(-item.cost); break;
                }

                if (item.item != null)
                {
                    int _r = item.count;
                    while (_r > 0) { itemsData.AddItem(item.item); _r--; }
                }
                if (item.currencyItem != null)
                {
                    switch (item.currencyItem.currency)
                    {
                        case Currency.Coins: heroBase.ModifyCoins(item.currencyItem.amount); break;
                        case Currency.Diamonds: heroBase.ModifyDiamonds(item.currencyItem.amount); break;
                    }
                }
            }
            else
            {
                GameManager.instance.errorPopup.SetActive(true);
            }
            Close();
        }
        if (itemsPack != null)
        {
            bool _canbuy = false;
            switch (itemsPack.currency)
            {
                case Currency.Coins: _canbuy = itemsPack.cost <= heroBase.coins; break;
                case Currency.Diamonds: _canbuy = itemsPack.cost <= heroBase.diamonds; break;
                case Currency.Cash:
                case Currency.Video:
                    _canbuy = true; break;
            }
            if (_canbuy)
            {
                switch (itemsPack.currency)
                {
                    case Currency.Coins: heroBase.ModifyCoins(-itemsPack.cost); break;
                    case Currency.Diamonds: heroBase.ModifyDiamonds(-itemsPack.cost); break;
                }
                for (int i = 0; i < itemsPack.items.Length; i++)
                {

                    if (itemsPack.items[i].item != null)
                    {
                        int _r = itemsPack.items[i].count;
                        while (_r > 0) { itemsData.AddItem(itemsPack.items[i].item); _r--; }
                    }
                    if (itemsPack.items[i].currencyItem != null)
                    {
                        switch (itemsPack.items[i].currencyItem.currency)
                        {
                            case Currency.Coins: heroBase.ModifyCoins(itemsPack.items[i].currencyItem.amount); break;
                            case Currency.Diamonds: heroBase.ModifyDiamonds(itemsPack.items[i].currencyItem.amount); break;
                        }
                    }
                }
            }
            Close();
        }

        Close();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
