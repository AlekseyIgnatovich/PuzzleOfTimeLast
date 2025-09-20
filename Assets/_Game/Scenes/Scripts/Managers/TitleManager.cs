using UnityEngine;
using TMPro;
using System;

public class TitleManager : Manager
{
    [SerializeField] TitleBtnController[] buttons;
    [SerializeField] TextMeshProUGUI titanTimerTMP;
    [SerializeField] TextMeshProUGUI rewardTimerTMP;
    [SerializeField] TitleBtnController rewardTitleBtn;
    [SerializeField] TitleBtnController titanTitleBtn;
    [SerializeField] TitleBtnController dalyDealTitleBtn;
    [SerializeField] TitleBtnController goldenPassTitleBtn;

    bool rewardReady;
    RewardManager rewardManager;
    FirestoreManager firestoreManager;
    DateTime claimedDateTime;
    DateTime nextRewardDateTime;
    TitanData titanData;

    bool showTimer;

    // private void OnEnable()
    // {
    //     RewardManager.onDailyRewardAchieved += ActivateTimer;
    // }

    // private void OnDisable()
    // {
    //     RewardManager.onDailyRewardAchieved -= ActivateTimer;
    // }

    private void Start()
    {
        Debug.Log("Opening HomeScreen");
        rewardManager = gameManager.rewardManager;
        firestoreManager = FirebaseManager.instance.firestoreManager;
        titanData = firestoreManager.titanData;
        rewardReady = !rewardManager.currentRwrdClaimed;
        if (!rewardReady)
        {
            Debug.Log("la reward no fue Claimeada");
        }
        rewardTimerTMP.gameObject.SetActive(false);
        titanTimerTMP.gameObject.SetActive(false);
        if (firestoreManager.dailyDealOn && !gameManager.dailyDealBtnPressed)
        {
            ActivateDailyDealNotification();
        }
        //rewardManager.LoadPlayerData();
        titanTitleBtn.Init(titanData.GetTitanTimer() <= 0 && !gameManager.titanBtnPressed);
        rewardTitleBtn.Init(!firestoreManager.rewardOn && !gameManager.rwrdBtnPressed);
        dalyDealTitleBtn.Init(firestoreManager.dailyDealOn && !gameManager.dailyDealBtnPressed);
        goldenPassTitleBtn.Init(firestoreManager.goldenPassOn && !gameManager.goldenPassBtnPressed);
        gameManager.universalMenuController.SetState(MenuState.Subscription);
    }

    public void ActivateRewardTimer(bool _activate)
    {
        rewardTimerTMP.gameObject.SetActive(_activate);
        rewardReady = !_activate;
        showTimer = _activate;
        CalculateNextRewardDate();
        if (!_activate && !gameManager.rwrdBtnPressed)
        {
            rewardTitleBtn.Init(true);
        }
    }

    void CalculateNextRewardDate()
    {
        claimedDateTime = firestoreManager.playerData.rewardData.lastDayClaimed;
        Debug.Log("Claimed date: " + claimedDateTime);
        nextRewardDateTime = claimedDateTime.AddHours(24); // Asignamos el resultado de AddHours
        Debug.Log("Este es el próximo reward: " + nextRewardDateTime);
    }

    public void ActivateDailyDealNotification()
    {
        dalyDealTitleBtn.Init(true);
    }

    public void ActivateGoldenPassNotification()
    {
        goldenPassTitleBtn.Init(true);
    }

    private void Update()
    {
        if (!rewardReady && showTimer)
        {
            rewardTimerTMP.text = ShowTimeLeftForNextReward();
        }

        float _timer = titanData.GetTitanTimer();

        if (_timer > 0)
        {
            if (!titanTimerTMP.gameObject.activeSelf)
            {
                titanTitleBtn.Init(false);
                titanTimerTMP.gameObject.SetActive(true);
            }
            float _min = Mathf.Floor(_timer / 60);
            //titanTimerTMP.text = $"{_min}:{Mathf.Floor(_timer - (_min * 60))}";

        }
        else
        {
            if (titanTimerTMP.gameObject.activeSelf)
            {
                Debug.Log("Activando titan");
                showTimer = true;
                titanTimerTMP.gameObject.SetActive(false);
                titanTitleBtn.Init(true);
                gameManager.titanBtnPressed = false;
            }
        }
    }

    public void ActivateTitanBtnNotification()
    {
        titanTitleBtn.Init(true);
    }

    public string ShowTimeLeftForNextReward()
    {
        TimeSpan timeRemaining = nextRewardDateTime - DateTime.Now;

        // Si el tiempo ya pasó, evitar que el resultado sea negativo
        if (timeRemaining.TotalSeconds < 0)
        {
            showTimer = false;
            rewardReady = true;
            gameManager.rewardManager.SelectDailyReward();
        }

        // Considerar el total de horas incluyendo días
        int totalHours = (int)timeRemaining.TotalHours;

        string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                             totalHours,
                                             timeRemaining.Minutes,
                                             timeRemaining.Seconds);
        return timeFormatted;
    }

    public void InitializeTitanTimer()
    {
        firestoreManager.titanData.GetTitanTimer();
        return;
    }
    public void ButtonHeroUnlock()
    {
        //gameManager.OpenHeroSummonScreen();
        gameManager.SetState(GameState.HeroUnlock);
    }
    public void ButtonGoldenPass()
    {
        gameManager.OpenGoldenPassWindow();
        return;
        //gameManager.SetState(GameState.Title);
    }
    // public void InitializeTitleButtons()
    // {
    //     foreach (var btn in buttons)
    //     {
    //         btn.Initialize();
    //     }
    // }
    public void ButtonRewards()
    {
        gameManager.OpenDailyRewardWindow();
        return;
        //gameManager.SetState(GameState.Title);
    }
    public void ButtonTitanBattle()
    {
        int titanIndex = gameManager.levelsData.levels.Length - 1;
        gameManager.levelsData.SelectLevel(titanIndex);
        gameManager.SetState(GameState.TitanSelect);
    }
    public void ButtonDailySpin()
    {
        gameManager.OpenDailySpinWindow();
        return;
    }
    public void ButtonDailyDeal()
    {
        gameManager.OpenDailyDealWindow();
        return;
        //gameManager.SetState(GameState.Title);
    }

    public void BuyDiamonds()
    {
        gameManager.SetState(GameState.DiamondsShop);
    }

    public void SetTitanTimer(string _time)
    {
        titanTimerTMP.text = _time;
    }

    public void SetRewardTimer(string _time)
    {
        rewardTimerTMP.text = _time;
    }

}
