using Firebase.Auth;
using UnityEngine;

public class Level : MonoBehaviour
{

    public string Name;
    public bool titanLevel;
    public bool modSafe;
    public bool reviveWithAds = true;
    [Space]
    public MobWave[] waves;
    [SerializeField] SpriteRenderer background;
    public CutsceneType cutsceneType;
    public CutscenePack[] cutscenes = new CutscenePack[3];
    [Space]
    [TextArea(4, 10)]
    public string description;
    [Space]
    public Sprite picture;
    [Space]
    public int maxCardPicks;
    [Range(0, 1f)]
    public float chanseToGetCoins;
    public float minimumCoinsAmount;
    public float maximumCoinsAmount;
    [Range(0, 1f)]
    public float chanseToGetDiamonds;
    public float minimumDiamondsAmount;
    public float maximumDiamondsAmount;
    [Space]
    public RewardPackage[] cardsPackages;
    public int packageSelect;
    public float coinsAmount;
    public float diamondsAmount;
    public Sprite backgroundSprite;

    // public void UpdateBackground(int _index)
    // {
    //     _index = Mathf.Clamp(_index, 0, waves.Length);
    //     background.sprite = waves[_index].background;
    // }

    private void Start()
    {
        if (backgroundSprite == null) backgroundSprite = waves[0].background;
        background.sprite = backgroundSprite;
    }

    public void RandomizeLoot()
    {
        for (int i = 0; i < cardsPackages.Length; i++)
        {
            cardsPackages[i].RandomiseChoise();
        }
        packageSelect = Random.Range(0, cardsPackages.Length);
        coinsAmount = Mathf.Floor(Random.Range(minimumCoinsAmount, maximumCoinsAmount));
        diamondsAmount = Mathf.Floor(Random.Range(minimumDiamondsAmount, maximumDiamondsAmount));
    }
    public void RandomizePackage()
    {
        packageSelect += Random.Range(0, cardsPackages.Length);
        if (cardsPackages.Length == 0) return;
        if (packageSelect >= cardsPackages.Length) { packageSelect -= cardsPackages.Length; }
        packageSelect = Mathf.Clamp(packageSelect, 0, cardsPackages.Length - 1);
        cardsPackages[packageSelect].RandomiseChoise();
    }
    public RewardPackage GetCardPackage()
    {
        if (cardsPackages.Length < packageSelect)
        {
            return cardsPackages[packageSelect];
        }
        else
        {
            return null;
        }
    }

    public CutscenePack GetCutscene()
    {
        return cutscenes[(int)cutsceneType];
    }

    public void SetAllResistances(float _val)
    {

        if (modSafe) { return; }

        EnemyPoint[] _enmp;
        Resistance[] _rss;
        for (int i = 0; i < waves.Length; i++)
        {
            _enmp = waves[i].enemy;

            for (int j = 0; j < _enmp.Length; j++)
            {
                _rss = _enmp[j].mob.resistances;

                for (int k = 0; k < _rss.Length; k++)
                {
                    _rss[k].modifier = _val;
                }
            }
        }
    }

    public int GenerateValueForLevel(string levelName, int maxValue)
    {
        // Convert the level name to a unique hash code (deterministic)
        int levelHash = levelName.GetHashCode();

        // Apply some variation by multiplying the hash and adding an arbitrary prime number
        int variedHash = levelHash * 31 + 7;

        // Ensure the value is positive using Math.Abs and constrain it within the desired range
        int randomValue = Mathf.Abs(variedHash) % maxValue;

        return randomValue; // Return the value between 0 and maxValue-1
    }
}
