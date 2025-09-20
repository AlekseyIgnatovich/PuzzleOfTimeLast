using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Drawing.Drawing2D;

//This script is for the reward panel after winning a level (It works in the similar way as the LootboxRewardPanel)
public class RewardPanel : MonoBehaviour
{

    [SerializeField] GameSettings settings;
    [SerializeField] HeroBase heroBase;
    [SerializeField] ItemsData itemsData;
    public GameplayManager manager;
    [SerializeField] Sprite[] frames;
    [SerializeField] Sprite[] elements;
    [Space]
    [SerializeField] Transform rewardsGrid;
    [SerializeField] TextMeshProUGUI rewardText;
    [Space]
    [SerializeField] GameObject cardShow;
    [SerializeField] GameObject itemShow;
    [SerializeField] GameObject coinsShow;
    [SerializeField] GameObject diamondsShow;
    [Space]
    [SerializeField] GameObject collectButton;
    [SerializeField] GameObject doneButton;
    [Space]
    [Header("Hero Card")]
    [SerializeField] Image cardFrame;
    [SerializeField] Image cardPicture;
    [SerializeField] Image cardElement;
    [SerializeField] TextMeshProUGUI textCardName;
    [SerializeField] TextMeshProUGUI textCardTier;
    [SerializeField] TextMeshProUGUI textCardHealth;
    [SerializeField] TextMeshProUGUI textCardAttack;
    [SerializeField] TextMeshProUGUI textCardDefense;
    [SerializeField] TextMeshProUGUI textCardDescription;
    [Space]
    [Header("Item")]
    [SerializeField] Image itemPicture;
    [SerializeField] TextMeshProUGUI textItemName;
    [SerializeField] TextMeshProUGUI textItemDescription;
    [Space]
    [Header("Currencies")]
    [SerializeField] TextMeshProUGUI textCoinsAmount;
    [SerializeField] TextMeshProUGUI textDiamondsAmount;

    int rewardsCount;
    bool getCoins;
    bool getDiamonds;

    HeroCard randomHero;
    bool hasGivenRandomHero;


    private void OnEnable()
    {

    }

    private void Start()
    {
        Debug.Log("Showing reward");
        manager.gameManager.levelsData.Save();

        Level _level = manager.level;
        rewardsCount = _level.cardsPackages.Length;
        getCoins = Random.value < _level.chanseToGetCoins;
        getDiamonds = Random.value < _level.chanseToGetDiamonds;

        collectButton.SetActive(true);
        doneButton.SetActive(false);
        _level.RandomizeLoot();
        //This will add the random hero reward to the count
        rewardsCount++;
        ActivatePackage();
    }

    public void OnCollectRewards()
    {
        if ((rewardsCount < 0) && !getCoins && !getDiamonds) { return; }

        TextMeshProUGUI _text = Instantiate(rewardText, rewardsGrid);
        Level _level = manager.level;
        if (!hasGivenRandomHero)
        {
            heroBase.AddCardToInventory(randomHero);
            _text.text = $"+ {randomHero.Name} Card";
            hasGivenRandomHero = true;
        }
        //recive package
        if (rewardsCount > 0)
        {
            rewardsCount--;
            if (rewardsCount >= 1)
            {
                HeroCard _crd = _level.cardsPackages.Length > 0 ? _level.cardsPackages[0].GetCard() : null;
                ItemObject _itm = _level.cardsPackages.Length > 0 ? _level.cardsPackages[0].GetItem() : null;

                if (_crd != null)
                {
                    heroBase.AddCardToInventory(_crd);
                    _text.text = $"+ {_crd.Name} Card";
                }
                else if (_itm != null)
                {
                    itemsData.AddItem(_itm);
                    _text.text = $"+ {_itm.item.Name} Item";
                }

            }

        }
        else
        {
            if (getCoins)
            {
                getCoins = false;
                heroBase.ModifyCoins(_level.coinsAmount);
                _text.text = $"+ {_level.coinsAmount.ToString("F0")} Coins";
            }
            else
            {
                if (getDiamonds)
                {
                    getDiamonds = false;
                    heroBase.ModifyDiamonds(_level.diamondsAmount);
                    _text.text = $"+ {_level.diamondsAmount.ToString("F0")} Diamonds";
                }
            }
        }

        manager.level.RandomizePackage();


        //reduce count, if count is done exit, else activate new package
        if ((rewardsCount <= 0) && !getCoins && !getDiamonds)
        {
            collectButton.SetActive(false);
            doneButton.SetActive(true);
            cardShow.SetActive(false);
            itemShow.SetActive(false);
            coinsShow.SetActive(false);
            diamondsShow.SetActive(false);
            rewardsGrid.gameObject.SetActive(true);
        }
        else
        {
            ActivatePackage();
        }
    }

    void ActivatePackage()
    {
        if (!hasGivenRandomHero)
        {
            cardShow.SetActive(false);
            itemShow.SetActive(false);
            coinsShow.SetActive(false);
            diamondsShow.SetActive(false);

            Rarity _heroRarity = Random.Range(0, 1) > 0.5f ? Rarity.Common : Rarity.Uncommon;
            Debug.Log("Random hero Rarity: " + _heroRarity);
            randomHero = heroBase.GetRandomCard(_heroRarity);
            if (randomHero == null) ActivatePackage();
            Debug.Log("Random hero: " + randomHero.Name);

            cardFrame.sprite = frames[(int)randomHero.rarity];
            cardElement.sprite = elements[(int)randomHero.type];
            cardPicture.sprite = randomHero.sprite;
            textCardName.text = randomHero.Name;
            textCardTier.text = randomHero.rarity.ToString();
            textCardHealth.text = $"{randomHero.maxHealth.ToString("F0")}";
            textCardAttack.text = $"{randomHero.attack.ToString("F0")}";
            textCardDefense.text = $"{randomHero.defense.ToString("F0")}";
            textCardDescription.text = randomHero.heroClass.description;
            GameManager.instance.ShowHeroCardAnimation(randomHero);
            cardShow.SetActive(true);
            return;
        }
        if (rewardsCount > 1)
        {

            cardShow.SetActive(false);
            itemShow.SetActive(false);
            coinsShow.SetActive(false);
            diamondsShow.SetActive(false);

            if (manager.level.GetCardPackage() != null)
            {
                Debug.Log("tiene package: !" + manager.level.GetCardPackage());
                HeroCard _crd = manager.level.GetCardPackage().GetCard();
                ItemObject _itm = manager.level.GetCardPackage().GetItem();
                if (_crd != null)
                {
                    cardShow.SetActive(true);

                    cardFrame.sprite = frames[(int)_crd.rarity];
                    cardElement.sprite = elements[(int)_crd.type];
                    cardPicture.sprite = _crd.sprite;
                    textCardName.text = _crd.Name;
                    textCardTier.text = _crd.rarity.ToString();
                    textCardHealth.text = $"{_crd.maxHealth.ToString("F0")}";
                    textCardAttack.text = $"{_crd.attack.ToString("F0")}";
                    textCardDefense.text = $"{_crd.defense.ToString("F0")}";
                    textCardDescription.text = _crd.heroClass.description;
                }
                else if (_itm != null)
                {
                    itemShow.SetActive(true);

                    itemPicture.sprite = _itm.item.sprite;
                    textItemName.text = _itm.item.Name;
                    textItemDescription.text = _itm.item.effect.description;
                }
            }


        }
        else
        {
            if (getCoins)
            {
                cardShow.SetActive(false);
                itemShow.SetActive(false);
                coinsShow.SetActive(true);
                diamondsShow.SetActive(false);
                textCoinsAmount.text = $"+{manager.level.coinsAmount.ToString("F0")}";
            }
            else if (getDiamonds)
            {
                cardShow.SetActive(false);
                itemShow.SetActive(false);
                coinsShow.SetActive(false);
                diamondsShow.SetActive(true);
                textDiamondsAmount.text = $"+{manager.level.diamondsAmount.ToString("F0")}";

            }
            else
            {
            }
        }
    }

    public void CollectingDone()
    {
        Destroy(gameObject);
        manager.gameManager.SetState(GameState.WorldMap);
    }

    public void Setup(GameplayManager _gpm)
    {
        manager = _gpm;
    }
}
