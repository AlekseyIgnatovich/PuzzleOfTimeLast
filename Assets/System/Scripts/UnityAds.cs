using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour, IUnityAdsInitializationListener
{

    [SerializeField] UnityRewardAds rewardAds;
    [Space]
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;
    [Space]
    public bool adLoaded = false;
    public bool showAds = false;
    public float timer;

    ShopItem item;
    AdRewardType rewardType;

    void Awake()
    {
        InitializeAds();
    }

    private void Update()
    {
        if (showAds)
        {
            if (adLoaded)
            {
                showAds = false;
                rewardAds.ShowAd(rewardType, item);
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (!adLoaded)
                {
                    timer = 1f;
                    rewardAds.LoadAd();
                }
            }
        }
    }

    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        SetupForAdLoad();
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        InitializeAds();
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void SetupForReward(AdRewardType _type, ShopItem _item)
    {
        item = _item;
        rewardType = _type;
        showAds = true;
        print("setting up for a reward");
    }

    public void SetupForAdLoad()
    {
        timer = 1f;
        rewardAds.LoadAd();
    }

    public void RewardedAd(System.Action _onComplete)
    {
        if (!adLoaded) return;
        rewardAds.ShowAdRewarded(_onComplete);
    }
}