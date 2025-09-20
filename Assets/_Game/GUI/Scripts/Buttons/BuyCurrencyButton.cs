using UnityEngine;
using UnityEngine.Purchasing;

public class BuyCurrencyButton : MonoBehaviour
{

    [SerializeField] ShopManager manager;
    [SerializeField] CodelessIAPButton iapButton;
    [SerializeField] float costUSD;
    [SerializeField] float diamonds;
    [Space]
    [SerializeField] bool checkMETADATA;

    private void Start()
    {
        if (!checkMETADATA) { return; }

        foreach (var product in iapButton.productId)
        {
            Debug.Log(product);
        }
    }

    public void BuyDiamondsPrompt()
    {
        manager.BuyDiamondsPopup(costUSD, diamonds);
    }

}
