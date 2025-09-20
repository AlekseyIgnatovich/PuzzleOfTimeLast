using UnityEngine;

public class AdsHeroRevival : MonoBehaviour {

    [SerializeField] GameplayManager manager;
    [Space]
    [SerializeField] float timerOffSet = .3f;

    float timer;

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                gameObject.SetActive(false);
            }
        }
    }

    public void ShowAds() {
        timer = timerOffSet;
        manager.gameManager.ads.SetupForReward(AdRewardType.HeroRevival, null);
    }

}
