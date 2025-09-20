using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldenPassManager : MonoBehaviour
{
    [SerializeField] GoldenPassItemController[] premiumItems, freeItems;
    [SerializeField] float goldenPassPrice;
    [SerializeField] CanvasGroup premiumCanvasGroup;
    [SerializeField] Slider bar;
    [SerializeField] GameObject goldenPassScreen, goldenPassNotificationPopup;
    [SerializeField] PopupAnimationController passPopup;
    [SerializeField] OnBoardingInstruction instruction;

    public int[] freeLvIndexs;
    public int[] premiumLvIndex;

    [Header("Debug")]
    [SerializeField] bool debug_setAllItems;

    FirestoreManager firestoreManager => FirebaseManager.instance.firestoreManager;
    public int premiumRewardsAmount => 20;
    public int freeRewardsAmount => 40;

    float progressBarOffset = 0.021f;
    float progressBarSum = 0.0515f;
    bool isPremium;

    // private void Start()
    // {
    //     for (int i = 0; i < freeItems.Length; i++)
    //     {
    //         freeLvIndexs[i] = freeItems[i].level;
    //     }
    //     for (int i = 0; i < premiumItems.Length; i++)
    //     {
    //         premiumLvIndex[i] = premiumItems[i].level;
    //     }
    // }

    // ---

    public void Initialize(bool _premium)
    {
        isPremium = _premium;
        premiumCanvasGroup.alpha = isPremium ? 1f : 0.4f;
        premiumCanvasGroup.interactable = isPremium;
    }

    private void OnEnable()
    {
        GameManager.onLevelFinished += OnLevelFinished;
        for (int i = 0; i < premiumItems.Length; i++) premiumItems[i].onClaim += OnClaimItem;
        for (int i = 0; i < freeItems.Length; i++) freeItems[i].onClaim += OnClaimItem;

    }
    private void OnDisable()
    {
        GameManager.onLevelFinished -= OnLevelFinished;
        for (int i = 0; i < premiumItems.Length; i++) premiumItems[i].onClaim -= OnClaimItem;
        for (int i = 0; i < freeItems.Length; i++) freeItems[i].onClaim -= OnClaimItem;
    }
    private void OnValidate()
    {
        if (debug_setAllItems)
        {
            debug_setAllItems = false;

            for (int i = 0; i < premiumItems.Length; i++)
            {
                int level = (i + 1) * 10;
                premiumItems[i].gameObject.name = "Premium Item - Lv." + level.ToString();
                premiumItems[i].level = level;
            }
            for (int i = 0; i < freeItems.Length; i++)
            {
                int level = (i + 2) * 5;
                freeItems[i].gameObject.name = "Free Item - Lv." + level.ToString();
                freeItems[i].level = level;
            }
        }
    }

    // ---

    public void ToggleActivateScreen(bool _active)
    {
        goldenPassScreen.SetActive(_active);
        if (_active)
        {
            Debug.Log("Showing golden pass");
            UpdateAllTiers();

            if (GameManager.instance.isFirstGamePass)
            {
                instruction.gameObject.SetActive(true);
                GameManager.instance.isFirstGamePass = false;
                PlayerPrefs.SetInt("Is_First_GamePass", 1);
            }
            else if (!isPremium)
            {
                passPopup.gameObject.SetActive(true);
                passPopup.Initialize(false);
                passPopup.ShowPopupWindow();
            }
        }
    }

    void UpdateAllTiers()
    {
        float calculatedOffset = firestoreManager.playerData.goldenPassData.currentPassLevel >= 10 ? progressBarOffset : 0f;
        float claculatedSum = firestoreManager.playerData.goldenPassData.currentPassLevel < 10 ? 0f : (progressBarSum / 10f) * (firestoreManager.playerData.goldenPassData.currentPassLevel - 10);
        bar.value = claculatedSum + calculatedOffset;
        for (int i = 0; i < premiumItems.Length; i++)
        {
            premiumItems[i].SetState(premiumItems[i].level <= firestoreManager.playerData.goldenPassData.currentPassLevel, firestoreManager.playerData.goldenPassData.premiumClaimed[i]);
        }
        for (int i = 0; i < freeItems.Length; i++)
        {
            freeItems[i].SetState(freeItems[i].level <= firestoreManager.playerData.goldenPassData.currentPassLevel, firestoreManager.playerData.goldenPassData.freeClaimed[i]);
        }
    }

    // --- 

    void OnLevelFinished(string _map, int _level)
    {
        if (firestoreManager.playerData.goldenPassData.currentPassLevel >= 200) return;

        firestoreManager.playerData.goldenPassData.currentPassLevel++;
        firestoreManager.UpdatePlayerDatabase();
        UpdateAllTiers();

        for (int i = 0; i < freeItems.Length; i++)
        {
            if (firestoreManager.playerData.goldenPassData.currentPassLevel == freeItems[i].level)
            {
                firestoreManager.goldenPassOn = true;
                //goldenPassNotificationPopup.SetActive(true);
                break;
            }
        }
    }

    void OnClaimItem(GoldenPassItemController _item)
    {
        bool free = false;
        int idx = 0;
        for (int i = 0; i < freeItems.Length; i++)
        {
            if (_item == freeItems[i])
            {
                free = true;
                idx = i;
                break;
            }
        }
        if (!free)
        {
            for (int i = 0; i < premiumItems.Length; i++)
            {
                if (_item == premiumItems[i])
                {
                    idx = i;
                    break;
                }
            }
        }

        if (free)
        {
            firestoreManager.playerData.goldenPassData.freeClaimed[idx] = true;
        }
        else
        {
            if (FirebaseManager.instance.gameManager.rewardManager.heroBase.diamonds >= _item.diamondsPrice)
            {
                firestoreManager.playerData.goldenPassData.premiumClaimed[idx] = true;
                FirebaseManager.instance.gameManager.rewardManager.heroBase.ModifyDiamonds(-_item.diamondsPrice);
            }
            else
            {
                return;
            }
        }
        FirebaseManager.instance.gameManager.rewardManager.CollectReward(_item.reward);
        UpdateAllTiers();
    }


    //This is called from the popup window button
    public void PurchaseGoldenPass()
    {
        Debug.Log("Golden pass purchased");
        //TO DO: Call the IAP Event to charge the user the goldenPassPrice amount;
        bool hasPurchased = true; // this bool will be assigned with the result of the IAP purchase method called
        if (hasPurchased)
        {
            FirebaseManager.instance.firestoreManager.playerData.goldenPassData.isPremium = isPremium = hasPurchased;
            FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
            Initialize(true);
        }
        else
        {
            Debug.Log("The golden pass can't be purchased");
        }
    }

}
