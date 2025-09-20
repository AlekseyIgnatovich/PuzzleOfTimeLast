using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailySpin : MonoBehaviour
{
    [Header("REWARD CHANCES")]
    [SerializeField] RewardChance[] rewards;

    [Header("Components")]
    [SerializeField] GameObject panel;
    [SerializeField] Button spinBtn, closeBtn;
    [SerializeField] GameObject lockedObj;
    [SerializeField] RectTransform[] wheelRects;
    [SerializeField] bool unlimitedSpins;

    FirestoreManager firestoreManager => FirebaseManager.instance.firestoreManager;
    int rewardIndex;
    float rewardItemAngle = 45f;

    // ---

    private void OnEnable()
    {
        spinBtn.onClick.AddListener(OnSpin);
        closeBtn.onClick.AddListener(OnClose);
    }
    private void OnDisable()
    {
        spinBtn.onClick.RemoveListener(OnSpin);
        closeBtn.onClick.RemoveListener(OnClose);
    }

    // ---

    public void OpenDailySpinUI()
    {
        panel.SetActive(true);
        CheckAvailable();
        for (int i = 0; i < wheelRects.Length; i++) wheelRects[i].rotation = Quaternion.identity;
    }
    void CheckAvailable()
    {
        TimeSpan difference = DateTime.Now - firestoreManager.playerData.lastDailySpinTime;
        bool available = difference.TotalHours >= 24;
        available = unlimitedSpins;
        spinBtn.interactable = available;
        lockedObj.SetActive(!available);
    }

    void OnSpin()
    {
        closeBtn.interactable = false;
        spinBtn.interactable = false;
        lockedObj.SetActive(true);
        firestoreManager.playerData.lastDailySpinTime = DateTime.Now;
        PickReward();
        FirebaseManager.instance.gameManager.ads.RewardedAd(() =>
        {
            StartCoroutine(SpinWheelCoroutine());
        });
    }
    void PickReward()
    {
        List<int> picks = new List<int>();
        for (int i = 0; i < rewards.Length; i++)
        {
            for (int j = 0; j < rewards[i].chance; j++)
            {
                picks.Add(i);
            }
        }
        rewardIndex = picks[UnityEngine.Random.Range(0, picks.Count)];
    }
    IEnumerator SpinWheelCoroutine()
    {
        float animTime = 7f;

        float initialSpins = 360 * 4;
        float totalSpins = initialSpins + ((rewardItemAngle + 0.44f) * rewardIndex);
        for (int i = 0; i < wheelRects.Length; i++) LeanTween.rotateAround(wheelRects[i], Vector3.forward, totalSpins, animTime).setEaseOutQuint();

        yield return new WaitForSeconds(animTime);

        FirebaseManager.instance.gameManager.rewardManager.CollectReward(rewards[rewardIndex].reward);
        //print(1);

        closeBtn.interactable = true;
    }

    void OnClose()
    {
        panel.SetActive(false);
    }

    // ---

    [Serializable]
    public class RewardChance
    {
        public RewardPackage reward;
        [Range(1, 100)] public int chance;
        public Sprite rewardIcon => reward.GetIconSprite();
    }
}
