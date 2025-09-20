using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubscriptionManager : MonoBehaviour
{
    [SerializeField] PopupAnimationController popupWindow;
    [SerializeField] GameObject subscriptionBtn;

    [SerializeField] RewardPackage[] rewards;

    bool subscribed;

    private void Start()
    {
        popupWindow.gameObject.SetActive(false);
    }

    public void CheckPendingRewards()
    {
        subscribed = FirebaseManager.instance.firestoreManager.playerData.rewardData.subscribed;
        DateTime lastLogin = FirebaseManager.instance.firestoreManager.playerData.lastLogin;
        DateTime today = DateTime.Now;
        int daysPassed = (today - lastLogin).Days;
        popupWindow.Initialize(subscribed);

        if (subscribed)
        {
            for (int day = 0; day < daysPassed; day++)
            {
                CollectRewards();
            }
        }
    }

    public void ShowPopup()
    {
        if (subscribed) return;

        //LeanTween.cancel(subscriptionBtn);
        popupWindow.Initialize(false);
        popupWindow.gameObject.SetActive(true);
        //subscriptionBtn.gameObject.SetActive(false);
        popupWindow.ShowPopupWindow();
    }

    //This is called from the popup window button
    public void PurchaseSubscription()
    {
        Debug.Log("Subscription purchased");
        subscribed = true;
        FirebaseManager.instance.firestoreManager.playerData.rewardData.subscribed = subscribed;
        FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
        CollectRewards();
    }

    void CollectRewards()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            GameManager.instance.rewardManager.CollectReward(rewards[i]);
        }
    }
}
