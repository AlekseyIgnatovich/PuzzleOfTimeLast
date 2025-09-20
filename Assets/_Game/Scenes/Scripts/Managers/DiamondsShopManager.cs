using UnityEngine;
using UnityEngine.Purchasing;

public class DiamondsShopManager : Manager
{

    [SerializeField] BuyDiamondPopup popup;
    [SerializeField] HeroBase heroBase;

    public void BuyDiamondsPopup(float _costUSD, float _diamonds)
    {
        popup.Setup(_costUSD, _diamonds);
    }

    public void BuyDiamonds(Product _product)
    {

        heroBase.ModifyDiamonds((float)_product.definition.payout.quantity);
        //heroBase.AddDiamonds(300);

        print($"Bought [{(float)_product.definition.payout.quantity}] Diamonds");
    }
}
