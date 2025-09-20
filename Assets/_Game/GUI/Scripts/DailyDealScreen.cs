using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class DailyDealScreen : MonoBehaviour
{
    [SerializeField] Image[] itemImages;
    [SerializeField] TextMeshProUGUI[] descriptionTMP;
    [SerializeField] TextMeshProUGUI gemPriceTMP;
    [SerializeField] TextMeshProUGUI moneyPriceTMP;
    [SerializeField] DealData[] dailyDeals;
    [SerializeField] GameObject popUpWindow;
    [SerializeField] TextMeshProUGUI timerTMP;
    [SerializeField] TextMeshProUGUI titleTMP;
    [SerializeField] CodelessIAPButton iAPButton;
    //[SerializeField] TextMeshProUGUI popupTimerTMP;

    DealData currentDeal;
    int currentIndex;
    bool isShowingTimer;
    DateTime today, tomorrow;
    RewardManager rewardManager;
    GameObject gemBtn;
    GameObject moneyBtn;

    private void Start()
    {

    }

    private void OnEnable()
    {
        RewardManager.onDailyDealClaimed += ShowClaimedWindow;
        rewardManager = FirebaseManager.instance.gameManager.rewardManager;
        gemPriceTMP.transform.parent.gameObject.SetActive(currentDeal != null && !currentDeal.claimed);
        isShowingTimer = true;
        //Initialize();
    }
    private void OnDisable()
    {
        RewardManager.onDailyDealClaimed -= ShowClaimedWindow;
        isShowingTimer = false;
    }

    private void Update()
    {
        //if (!isShowingTimer) return;

        timerTMP.text = ShowTimeLeftForTomorrow();
        //popupTimerTMP.text = timeFormatted;
    }

    public string ShowTimeLeftForTomorrow()
    {
        today = DateTime.Now;
        tomorrow = today.AddDays(1).Date;
        TimeSpan timeRemaining = tomorrow - today;

        string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                             timeRemaining.Hours,
                                             timeRemaining.Minutes,
                                             timeRemaining.Seconds);
        return timeFormatted;
    }
    private void SetPriceText(float _price)
    {
        gemPriceTMP.text = "$" + _price;
    }
    public void Initialize()
    {
        today = DateTime.Now.ToUniversalTime();
        DateTime initLogin = FirebaseManager.instance.firestoreManager.playerData.firstLogin.ToUniversalTime();
        TimeSpan difference = today - initLogin;
        Debug.Log("Last login: " + initLogin);
        Debug.Log("Difference days: " + difference.TotalDays);
        if (difference.TotalDays <= 0)
        {
            currentIndex = 0;
            Debug.Log("Todavia no paso un dia");
        }
        else
        {
            currentIndex = Mathf.RoundToInt((float)difference.TotalDays / 10);
            Debug.Log("Paso mas de un dia" + currentIndex);
        }
        gemBtn = gemPriceTMP.transform.parent.gameObject;
        moneyBtn = moneyPriceTMP.transform.gameObject;
        if (currentIndex >= dailyDeals.Length) currentIndex = 0;
        //FirebaseManager.instance.firestoreManager.dailyDealOn = !_claimed;
        dailyDeals[currentIndex].claimed = FirebaseManager.instance.firestoreManager.playerData.dailyDealData.hasClaimedDeal[currentIndex];
        currentDeal = dailyDeals[currentIndex];

        UpdateInfo();
    }

    // public void PassToNextDay()
    // {
    //     today = today.AddDays(1);
    //     tomorrow = today.AddDays(1).Date;
    //     currentIndex = (int)today.DayOfWeek;
    //     FirebaseManager.instance.firestoreManager.dailyDealOn = true;
    //     dailyDeals[currentIndex].claimed = false;
    //     currentDeal = dailyDeals[currentIndex];
    //     UpdateInfo();
    // }

    void UpdateInfo()
    {
        titleTMP.text = currentDeal.dealTitle;
        SetPriceText(currentDeal.price);
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (currentDeal.items[i].icon != null)
            {
                itemImages[i].sprite = currentDeal.items[i].icon;
                // Mantener las proporciones del sprite
                float spriteHeight = itemImages[i].sprite.rect.height;
                float spriteWidth = itemImages[i].sprite.rect.width;
                float aspectRatio = spriteWidth / spriteHeight;

                // Fijar el alto de la imagen
                float targetHeight = itemImages[i].rectTransform.rect.height;

                // Calcular la nueva anchura basada en la relación de aspecto
                float targetWidth = targetHeight * aspectRatio;

                // Aplicar el nuevo tamaño respetando las proporciones
                itemImages[i].rectTransform.sizeDelta = new Vector2(targetWidth, targetHeight);
            }
            else if (currentDeal.items[i].type != RewardType.Coins || currentDeal.items[i].type != RewardType.Diamonds)
            {
                Debug.Log("Buscando Icono");
                itemImages[i].sprite = currentDeal.items[i].GetIconSprite();
            }
            string _description = "";
            if (currentDeal.items[i].type == RewardType.Diamonds || currentDeal.items[i].type == RewardType.Coins)
            {
                _description = "+";
            }
            else
            {
                _description = "x";
            }
            _description += currentDeal.items[i].description;

            descriptionTMP[i].text = _description;
        }
        if (currentDeal.currency == CurrencyType.Gems)
        {
            gemPriceTMP.text = currentDeal.price.ToString();
        }
        else if (currentDeal.currency == CurrencyType.Money)
        {
            moneyPriceTMP.text = "$" + currentDeal.price.ToString();
        }

        //if (currentDeal.claimed) return;
        moneyBtn.SetActive(currentDeal.currency == CurrencyType.Money);
        gemBtn.SetActive(currentDeal.currency == CurrencyType.Gems);
    }

    public void SetCurrentDailyDeal()
    {

    }

    //This function is called by the Daily Deal Screen Button
    public void ClaimDeal()
    {
        Debug.Log("Claiming Deal: " + currentDeal);
        //if (currentDeal.claimed) return;
        FirebaseManager.instance.firestoreManager.dailyDealOn = false;
        currentDeal.claimed = true;
        rewardManager.ClaimCurrentDailyDeal(currentDeal);
    }

    public void ShowClaimedWindow()
    {
        Debug.Log("Deal Claimed");
        StartCoroutine(ShowClaimedWindowCoroutine());
    }


    IEnumerator ShowClaimedWindowCoroutine()
    {
        currentDeal.claimed = true;
        popUpWindow.SetActive(true);
        yield return new WaitForSeconds(1);
        popUpWindow.SetActive(false);
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class DealData
{
    public string dealTitle;
    public RewardPackage[] items;
    public bool claimed;
    public float price;
    public CurrencyType currency;
}

public enum CurrencyType { Gems, Coins, Money }
