using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShopHeroButton : MonoBehaviour
{

    [SerializeField] ItemsData data;
    [SerializeField] HeroBase heroBase;
    [SerializeField] Manager manager;
    [SerializeField] BuyItemPopup buyPopup;
    [SerializeField] VideoCanvas videoCanvas;
    [SerializeField] GameObject videoButton;
    [Space]
    [SerializeField] GameObject[] currency;
    [SerializeField] GameObject unlockScreen;
    [SerializeField] GameObject heroesPanel;
    [SerializeField] HeroUnlockManager heroUnlockManager;
    [Space]
    public ShopItem shopItem;
    
    /// <summary>
    /// Fix later
    /// </summary>
    [SerializeField] ShopHeroReward heroRewardPanel;

    private void OnEnable()
    {
        data.OnSwitchCurrency += SetCurrency;
    }
    private void OnDisable()
    {
        data.OnSwitchCurrency -= SetCurrency;
    }
    private void Start()
    {
        videoButton.SetActive(shopItem.canBuyWithVideo);
        SetCurrency();
    }

    void SetCurrency()
    {
        int _index = (int)data.selectedCurrency;
        //We set the index to 1 always because the client wants to show only the diamonds price
        _index = 1;
        for (int i = 0; i < currency.Length; i++)
        {
            currency[i].SetActive(_index == i);
        }
    }
    public void ShowPurchasePopup()
    {
        ShopHeroItem _hi = shopItem as ShopHeroItem;
        _hi.Randomize();
        buyPopup.Setup(shopItem);
    }
    public void BuyWithVideo()
    {
        //videoCanvas.OpenVideo(shopItem);

        BuyWithVideoAsync();
    }

    private async UniTask BuyWithVideoAsync()
    {
        var result = await AdsManager.instance.ShowAddAsync(AdRewardType.unlock_hero);
        if (!result)
        {
            Debug.LogError("failed show ad");
            return;
        }
        
        var shopHeroItem = shopItem as ShopHeroItem;
        if (shopHeroItem != null)
        {
            var card = shopHeroItem.GetCard();
            heroBase.AddCardToInventory(card);
            heroRewardPanel.Setup(card);
        }
    }
    
    public void OpenHeroPackagePreview()
    {
        if (manager == null) { return; }

        data.heroUnlockPack = shopItem as ShopHeroItem;
        heroUnlockManager.Setup(data.heroUnlockPack);
        unlockScreen.SetActive(true);
        heroesPanel.SetActive(false);
        //manager.gameManager.SetState(GameState.HeroUnlock);
    }
}
