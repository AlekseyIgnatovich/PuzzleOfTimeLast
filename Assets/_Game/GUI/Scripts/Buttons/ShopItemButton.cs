using UnityEngine;

public class ShopItemButton : MonoBehaviour {

    [SerializeField] ItemsData data;
    [SerializeField] ShopManager manager;
    [SerializeField] BuyItemPopup buyPopup;
    [SerializeField] VideoCanvas videoCanvas;
    [Space]
    [SerializeField] GameObject[] currency;
    [Space]
    public ShopItem shopItem;

    private void OnEnable() {
        data.OnSwitchCurrency += SetCurrency;
    }
    private void OnDisable() {
        data.OnSwitchCurrency -= SetCurrency;
    }
    private void Start() {
        SetCurrency();
    }

    void SetCurrency() {
        int _index = (int)data.selectedCurrency;
        for (int i = 0; i < currency.Length; i++) {
            currency[i].SetActive(_index == i);
        }
    }
    public void SelectItem() {
        if (shopItem is ShopPlayItem) {
            ShopPlayItem _item = shopItem as ShopPlayItem;
            data.SelectItem(_item.item);
        }
    }
    public void ShowPurchasePopup() {
        buyPopup.Setup(shopItem);
    }
    public void BuyWithVideo() {
        manager.gameManager.ads.SetupForReward(AdRewardType.Item, shopItem);
        //videoCanvas.OpenVideo(shopItem);
    }
}
