using UnityEngine;
using Firebase.Firestore;
using System;
using System.Threading.Tasks;

public class RewardManager : MonoBehaviour
{
    public ItemsData itemsData;
    public HeroBase heroBase;
    public RewardPackage[] dailyRewards;
    [SerializeField] DailyRewardUI dailyRwrdUIScreen;
    [SerializeField] DailyDealScreen dailyDealScreen;
    public DateTime fakeToday;
    public bool isFakingTime;
    public bool currentRwrdClaimed;
    public bool hasUpdatedLogin;


    FirestoreManager firestoreManager => FirebaseManager.instance.firestoreManager;
    RewardPackage dailyReward;
    RewardBoxInfo[] rewardBoxInfos;
    int currentRewardIndex;


    //Events//------------------------------------------------------------------------
    public delegate void OnDailyRewardAchieved();
    public static OnDailyRewardAchieved onDailyRewardAchieved;

    public delegate void OnDailyDealClaimed();
    public static OnDailyDealClaimed onDailyDealClaimed;
    ////------------------------------------------------------------------------------

    private void OnEnable()
    {
        FirebaseManager.onUserLogin += LoadPlayerData;
    }

    private void OnDisable()
    {
        FirebaseManager.onUserLogin -= LoadPlayerData;
    }

    private void Update()
    {
        // if (Input.GetKey(KeyCode.D))
        // {
        //     Debug.Log("Diamonds Added");
        //     heroBase.ModifyDiamonds(10);
        // }
    }


    public void LoadPlayerData()
    {
        GameManager.instance.subscriptionManager.CheckPendingRewards();
        DateTime today = isFakingTime ? fakeToday : DateTime.Now;
        DateTime lastClaimed = firestoreManager.playerData.rewardData.lastDayClaimed;
        //Debug.Log("last claimed date: " + lastClaimed);

        TimeSpan difference = today - lastClaimed;

        Debug.Log("difference: " + difference.TotalDays);
        bool isLoginEveryDay = difference.TotalDays < 2;
        if (firestoreManager.playerData.lastLogin.Date < today.Date && !isLoginEveryDay)
        {
            Debug.Log("Has passed more than a day");
            firestoreManager.playerData.rewardData.currentDayIndex = 0;
            firestoreManager.playerData.rewardData.hasClaimedReward = new bool[FirebaseManager.instance.gameManager.rewardManager.dailyRewards.Length];
            firestoreManager.UpdatePlayerDatabase();
        }
        else if (isLoginEveryDay && difference.TotalDays >= 1)
        {
            firestoreManager.playerData.rewardData.currentDayIndex++;
        }
        else
        {
            Debug.Log("The player has claimed current reward");
        }

        if (firestoreManager.playerData.rewardData.currentDayIndex >= dailyRewards.Length)
        {
            firestoreManager.playerData.rewardData.currentDayIndex = 0;
        }
        currentRewardIndex = firestoreManager.playerData.rewardData.currentDayIndex;
        firestoreManager.rewardOn = !firestoreManager.playerData.rewardData.hasClaimedReward[currentRewardIndex];
        TitleManager titleManager;
        if (FirebaseManager.instance.gameManager.currentScreen.TryGetComponent<TitleManager>(out titleManager))
        {
            titleManager.ActivateRewardTimer(!firestoreManager.rewardOn);
        }

        SelectDailyReward();

        //firestoreManager.playerData.dailyDealData.claimed = false;
        if (firestoreManager.playerData.dailyDealData.claimed)
        {
            firestoreManager.dailyDealOn = false;
            //dailyDealScreen.Initialize();
            Debug.Log("El daily deal ya fue claimeado");
        }
        else
        {
            firestoreManager.dailyDealOn = true;
            //dailyDealScreen.Initialize();

            Debug.Log("El daily deal NO fue claimeado");

            if (FirebaseManager.instance.gameManager.currentScreen.TryGetComponent<TitleManager>(out titleManager))
            {
                titleManager.ActivateDailyDealNotification();
            }
        }
        dailyDealScreen.Initialize();
        UpdateLastLogin();
        //LeanTween.delayedCall(1f, UpdateLastLogin);
    }

    void UpdateLastLogin()
    {
        if (hasUpdatedLogin) return;
        hasUpdatedLogin = true;
        firestoreManager.playerData.lastLogin = DateTime.Now;
        firestoreManager.UpdatePlayerDatabase();
    }


    // This is called by the button to test the next Daily Reward
    public void PassToNextDay()
    {
        // if (!isFakingTime) isFakingTime = true;
        firestoreManager.playerData.rewardData.currentDayIndex++;
        firestoreManager.rewardOn = true;
        currentRewardIndex = firestoreManager.playerData.rewardData.currentDayIndex;
        //dailyDealScreen.PassToNextDay();
        if (!isFakingTime)
        {
            currentRewardIndex++;
            FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
        }
        // playerData = FirebaseManager.instance.firestoreManager.playerData;
        //fakeToday.AddHours(24);
        SelectDailyReward();
        //LoadPlayerData();
        Debug.Log("today is now: " + fakeToday);
    }

    public void SelectDailyReward()
    {
        dailyReward = dailyRewards[currentRewardIndex];

        rewardBoxInfos = new RewardBoxInfo[dailyRewards.Length];
        for (int i = 0; i < rewardBoxInfos.Length; i++)
        {
            rewardBoxInfos[i] = new RewardBoxInfo();
            rewardBoxInfos[i].reward = dailyRewards[i];
            if (i < currentRewardIndex)
            {
                rewardBoxInfos[i].hasBeenClaimed = firestoreManager.playerData.rewardData.hasClaimedReward[i];
            }
            else if (i == currentRewardIndex)
            {
                //dailyRewardBoxInfo = rewardBoxInfos[i];
                rewardBoxInfos[i].isCurrent = true;
                rewardBoxInfos[i].hasBeenClaimed = firestoreManager.playerData.rewardData.hasClaimedReward[i];
                currentRwrdClaimed = rewardBoxInfos[i].hasBeenClaimed;
                dailyRwrdUIScreen.claimBtn.SetActive(!currentRwrdClaimed);
                //TO DO: Activate this line below to activate timer
                //FirebaseManager.instance.gameManager.currentScreen.GetComponent<TitleManager>().ActivateRewardTimer(currentRwrdClaimed);
            }
            else if (i - 1 == currentRewardIndex)
            {
                rewardBoxInfos[i].isNextOne = true;
            }
        }
        dailyRwrdUIScreen.Initialize(rewardBoxInfos);
    }

    public void ClaimCurrentDailyReward()
    {
        Debug.Log("Reward claimed");
        Debug.Log("CurrentDailyIndex " + currentRewardIndex);
        rewardBoxInfos[currentRewardIndex].hasBeenClaimed = true;
        firestoreManager.playerData.rewardData.hasClaimedReward[currentRewardIndex] = true;
        firestoreManager.playerData.rewardData.lastDayClaimed = DateTime.Now;
        dailyRwrdUIScreen.Initialize(rewardBoxInfos);
        dailyRwrdUIScreen.claimBtn.SetActive(false);
        dailyRwrdUIScreen.dailyRewardPopUp.SetActive(true);
        CollectReward(dailyReward);
        onDailyRewardAchieved?.Invoke();
    }

    public void ClaimCurrentDailyDeal(DealData deal)
    {
        //if (firestoreManager.playerData.dailyDealData.hasClaimedDeal[currentRewardIndex]) return;

        if (deal.currency == CurrencyType.Gems && firestoreManager.playerData.currencyData.diamonds < deal.price) return;

        currentRwrdClaimed = true;
        firestoreManager.playerData.dailyDealData.hasClaimedDeal[currentRewardIndex] = true;
        if (deal.currency == CurrencyType.Gems)
        {
            heroBase.ModifyDiamonds(-deal.price);
        }
        for (int i = 0; i < deal.items.Length; i++)
        {
            CollectReward(deal.items[i]);
        }
        onDailyDealClaimed?.Invoke();
    }

    public void ToggleDailyRewardUIScreen(bool _active)
    {
        dailyRwrdUIScreen.gameObject.SetActive(_active);
    }

    public void ActivateDailyDealScreen()
    {
        dailyDealScreen.gameObject.SetActive(true);
    }

    public void CollectReward(RewardPackage _package)
    {
        if (_package as RewardItem != null)
        {
            RewardItem item = _package as RewardItem;
            item.RandomiseChoise();
            itemsData.AddItem(item.GetItem());
        }
        else if (_package as RewardCurrency)
        {
            RewardCurrency currency = _package as RewardCurrency;
            currency.RandomiseChoise();
            switch (currency.type)
            {
                case RewardType.Coins:
                    heroBase.ModifyCoins(currency.GetAmount());
                    break;
                case RewardType.Diamonds:
                    heroBase.ModifyDiamonds(currency.GetAmount());
                    break;
            }
        }
    }
}


[System.Serializable]
public class RewardBoxInfo
{
    public RewardPackage reward;
    public bool isCurrent;
    public bool hasBeenClaimed;
    public bool isNextOne;
}
