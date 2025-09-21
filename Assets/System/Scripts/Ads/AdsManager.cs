using System;
using Cysharp.Threading.Tasks;
using Unity.Services.LevelPlay;
using UnityEngine;
using LevelPlayAdDisplayInfoError = com.unity3d.mediation.LevelPlayAdDisplayInfoError;
using LevelPlayAdError = com.unity3d.mediation.LevelPlayAdError;
using LevelPlayAdInfo = com.unity3d.mediation.LevelPlayAdInfo;
using LevelPlayConfiguration = com.unity3d.mediation.LevelPlayConfiguration;
using LevelPlayInitError = com.unity3d.mediation.LevelPlayInitError;
using LevelPlayReward = com.unity3d.mediation.LevelPlayReward;


public class AdsManager : MonoBehaviour
{
    enum AdState
    {
        wait = 0,
        showing = 1,
        showing_failed = 2,
        reward_received = 3,
    }
    
#if UNITY_EDITOR
    string IRONSOURCE_APP_KEY = "23aa6738d"; //test key
#elif UNITY_ANDROID
    string IRONSOURCE_APP_KEY = "23aa6738d"; //test key
#elif UNITY_IOS
    string IRONSOURCE_APP_KEY = "23aa6738d"; //test key
#endif

    public static AdsManager instance => _instance;
    private static AdsManager _instance; 
    
    [SerializeField] bool _testMode = true;

    public bool IsAdReady
    {
        get
        {
            if (_rewardedAd == null)
                return false;

            return _rewardedAd.IsAdReady();
        }
    }

    AdState _adState;

    private ILevelPlayRewardedAd _rewardedAd;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        LevelPlay.Init(IRONSOURCE_APP_KEY, null, new []{com.unity3d.mediation.LevelPlayAdFormat.REWARDED} );
        
        LevelPlay.ValidateIntegration();
        LevelPlay.OnInitSuccess += OnInitSuccess;
        LevelPlay.OnInitFailed += (LevelPlayInitError obj) => Debug.LogError($"Init Failed: {obj.ErrorMessage}");
        LevelPlay.OnImpressionDataReady += ( obj) => Debug.LogError($"Impression Data Ready: {obj}");
        
        _adState = AdState.wait;
        
        _rewardedAd = new LevelPlayRewardedAd("adUnitId");
        _rewardedAd.LoadAd();

        _rewardedAd.OnAdRewarded += OnRewardSuccessShowed;
        _rewardedAd.OnAdDisplayFailed += OnAdDisplayFailed;
        _rewardedAd.OnAdLoadFailed += OnAdLoadFailed;
    }

    private void OnInitSuccess(LevelPlayConfiguration obj)
    {
        Debug.LogError($"OnInitSuccess: ");
    }
    
    private void OnRewardSuccessShowed(LevelPlayAdInfo arg1, LevelPlayReward arg2)
    {
        _adState = AdState.reward_received;
        Debug.LogError($"Reward Received: {arg1}");
    }
    
    private void OnAdLoadFailed(LevelPlayAdError obj)
    {
        _adState = AdState.showing_failed;
    }

    private void OnAdDisplayFailed(LevelPlayAdDisplayInfoError obj)
    {
        _adState = AdState.showing_failed;
    }
    
    public async UniTask<bool> ShowAddAsync(AdRewardType type)
    {
        //ToDo: add screen locker 
        
        Debug.Log($"Show Add async: {type}");
        
        if (_adState != AdState.wait)
        {
            Debug.LogError("Wrong ad state");
            return false;
        }
        
        if (!IsAdReady)
        {
            Debug.LogError("Unity Ads are not ready");
            return false;
        }
        _rewardedAd.ShowAd(type.ToString());
        _adState = AdState.showing;
        
        await UniTask.WaitUntil(() => _adState != AdState.showing);
        
        Debug.LogError($"ShowAdd finish with state: {_adState}");
        
        var success = _adState == AdState.reward_received;
        _adState = AdState.wait;
        _rewardedAd.LoadAd();

        return success;
    } 
}