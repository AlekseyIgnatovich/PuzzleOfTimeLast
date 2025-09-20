using UnityEngine;
using UnityEngine.Purchasing;

public class ShopManager : Manager
{

    [SerializeField] ItemsData itemsData;
    [SerializeField] HeroBase heroBase;
    [Space]
    [SerializeField] BuyItemPopup itemPopup;
    [SerializeField] BuyDiamondPopup popup;
    [SerializeField] GameObject[] shops;
    [SerializeField] GameObject[] panels;
    [SerializeField] ShopPanelButton[] panelButtons;
    [SerializeField] GameObject currencySwicth;

    private void OnEnable()
    {
        //itemsData.OnShowShop += ShowShop;
    }
    private void OnDisable()
    {
        //itemsData.OnShowShop -= ShowShop;
    }

    private void Start()
    {
        itemPopup.gameObject.SetActive(false);
        //ShowPanel(panels[3]);
    }

    // public void ShowShop()
    // {
    //     int _index = itemsData.shopShow;
    //     for (int i = 0; i < shops.Length; i++)
    //     {
    //         shops[i].SetActive(_index == i);
    //     }
    // }

    public void OpenShopFromTab()
    {
        itemPopup.gameObject.SetActive(false);
        ShowPanel(panels[2]);
        Debug.Log("Opening items panel");
    }

    public void OpenShopFromDiamondButton()
    {
        itemPopup.gameObject.SetActive(false);
        ShowPanel(panels[3]);
    }

    public void GoToTitle()
    {
        gameManager.ReturnToHome();
    }
    public void GoToMap()
    {
        gameManager.SetState(GameState.WorldMap);
    }

    public void ShowPanel(GameObject _panel)
    {
        ToggleActivateCurrencySwitch(_panel == panels[3]);
        Debug.Log("showing panel: " + _panel);
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(panels[i] == _panel);
        }
        UpdateButtons();
    }
    void UpdateButtons()
    {
        for (int i = 0; i < panelButtons.Length; i++)
        {
            panelButtons[i].UpdateButton();
        }
    }

    public void ToggleActivateCurrencySwitch(bool _active)
    {
        currencySwicth.SetActive(_active);
    }

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
