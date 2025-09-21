using Cysharp.Threading.Tasks;
using UnityEngine;

public class AdsHeroRevival : MonoBehaviour {

    [SerializeField] GameplayManager manager;
    [Space]
    [SerializeField] float timerOffSet = .3f;

    private float _timer;

    private void Update() {
        if (_timer > 0) {
            _timer -= Time.deltaTime;
            if (_timer <= 0) {
                gameObject.SetActive(false);
            }
        }
    }

    public void ShowAds() {
        _timer = timerOffSet;
        
        BuyWithVideoAsync();
    }

    private async UniTask BuyWithVideoAsync()
    {
        var result = await AdsManager.instance.ShowAddAsync(AdRewardType.revival_hero);
        if (!result)
        {
            Debug.LogError($"Failed to show ads reward: {result}");
            return;
        }
        
        var gameplayManager = GameManager.FindObjectOfType<GameplayManager>();
        if (gameplayManager == null)
        {
            Debug.LogError($"gameplayManager is null");
            return;
        }

        gameplayManager.ReviveHeroesWithAds();
    }
}
