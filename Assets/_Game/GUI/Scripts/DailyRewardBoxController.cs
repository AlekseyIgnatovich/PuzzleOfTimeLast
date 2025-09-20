using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardBoxController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountTMP;
    [SerializeField] GameObject glowImg;
    [SerializeField] GameObject claimedImg;
    [SerializeField] GameObject nextDayMessage;
    [SerializeField] RewardPackage reward;

    public RewardBoxInfo rewardBoxInfo;

    public delegate void OnRewardClaimed();
    public static OnRewardClaimed onRewardClaimed;

    private void Start()
    {

        // amountTMP = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        // glowImg = transform.GetChild(4).gameObject;
        // nextDayMessage = glowImg.transform.GetChild(0).gameObject;
        // claimedImg = transform.GetChild(5).gameObject;
    }

    public void Initialize(RewardBoxInfo _info)
    {
        rewardBoxInfo = _info;
        glowImg.SetActive(rewardBoxInfo.isCurrent);
        claimedImg.SetActive(rewardBoxInfo.hasBeenClaimed);
        nextDayMessage.SetActive(rewardBoxInfo.isNextOne);
        reward = rewardBoxInfo.reward;
    }

    public void ClaimReward()
    {
        FirebaseManager.instance.gameManager.rewardManager.CollectReward(reward);
        onRewardClaimed?.Invoke();
    }
}
