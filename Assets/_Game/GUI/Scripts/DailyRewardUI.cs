using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyRewardUI : MonoBehaviour
{
    [SerializeField] DailyRewardBoxController[] rewardBoxes;
    public GameObject dailyRewardPopUp;
    public GameObject claimBtn;

    private void Start()
    {
        dailyRewardPopUp.SetActive(false);
    }

    public void Initialize(RewardBoxInfo[] _rewards)
    {
        for (int i = 0; i < _rewards.Length; i++)
        {
            rewardBoxes[i].Initialize(_rewards[i]);
        }
    }
}

