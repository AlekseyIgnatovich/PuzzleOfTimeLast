using UnityEngine;
using UnityEngine.Advertisements;

public class UnityRewardAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms
    [Space]
    [SerializeField] GameManager gameManager;
    [SerializeField] UnityAds unityAds;
    [SerializeField] ItemsData itemsData;
    [SerializeField] HeroBase heroBase;
    [Space]
    [SerializeField] LotteryRewardPanel lotteryPanel;
    [SerializeField] ShopHeroReward heroRewardPanel;

    [HideInInspector] public ShopItem item;
    [HideInInspector] public AdRewardType rewardType;

    System.Action onComplete;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        unityAds.adLoaded = true;

        if (adUnitId.Equals(_adUnitId))
        {
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd(AdRewardType _type, ShopItem _item)
    {
        Debug.Log("Trying to show a reward ad");
        item = _item;
        rewardType = _type;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }
    public void ShowAdRewarded(System.Action _onComplete)
    {
        Debug.Log("Trying to show a reward ad");
        onComplete = _onComplete;
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.

            unityAds.SetupForAdLoad();
            GetReward();
            onComplete?.Invoke();
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void GetReward()
    {

        switch (rewardType)
        {
            case AdRewardType.Item://///////////////////////////////////////
                if (item == null) { return; }

                ShopPlayItem spi;
                ShopHeroItem shi;
                ShopLotteryItem sli;

                spi = item as ShopPlayItem;
                if (spi != null)
                {
                    itemsData.AddItem(spi.item);
                    return;
                }
                shi = item as ShopHeroItem;
                if (shi != null)
                {
                    heroBase.AddCardToInventory(shi.GetCard());
                    heroRewardPanel.Setup(shi.GetCard());
                    return;
                }
                sli = item as ShopLotteryItem;
                if (sli != null)
                {
                    lotteryPanel.Open(sli.item);
                    return;
                }
                break;

            case AdRewardType.HeroRevival:////////////////////////////////////
                GameplayManager _gpm = gameManager.currentScreen as GameplayManager;

                if (_gpm != null)
                {
                    _gpm.ReviveHeroesWithAds();
                }
                break;
        }
    }
}
