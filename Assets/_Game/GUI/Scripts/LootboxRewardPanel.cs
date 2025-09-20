using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class LootboxRewardPanel : MonoBehaviour
{

    GameManager manager;
    [SerializeField] HeroBase heroBase;
    [SerializeField] ItemsData itemsData;
    [Space]
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

    [SerializeField] int rewardsCount;
    [SerializeField] List<LootboxReward> rewards;
    [SerializeField] GameObject panel;

    public delegate void OnItemAdded();
    public static OnItemAdded onItemAdded;


    int index, lootboxIndex;
    int totalItemsAmount;
    int totalHeroesAmount;
    bool hasClaimedAllItems;
    bool hasClaimedAllHeroes;
    HeroCard card;
    ItemObject item;
    float coins;
    float diamonds;

    private void Start()
    {
        index = 0;
        lootboxIndex = 0;
        //totalItemsAmount = 0;
        //totalHeroesAmount = 0;
        hasClaimedAllItems = false;
        hasClaimedAllHeroes = false;
        //collectButton.SetActive(true);
        doneButton.SetActive(false);
        collectButton.SetActive(false);
        ActivatePackage();
    }

    public void OnCollectRewards()
    {
        Debug.Log("Collecting Reward: " + rewardsCount);
        if (GameManager.instance.isShowingHeroCard)
        {
            GameManager.instance.HideHeroCardAnimationScreen();
            GameManager.onHeroCardUnlocked?.Invoke();
            panel.SetActive(true);
        }
        rewardsCount--;

        TextMeshProUGUI _text = Instantiate(rewardText, rewardsGrid);

        if (card != null)
        {
            heroBase.AddCardToInventory(card);
            _text.text = $"+ {card.Name} Card";
        }
        if (item != null)
        {
            itemsData.AddItem(item);
            _text.text = $"+ {item.item.Name} Item";
        }
        if (coins > 0)
        {
            heroBase.ModifyCoins(coins);
            _text.text = $"+ {coins.ToString("F0")} Coins";
        }
        if (diamonds > 0)
        {
            heroBase.ModifyDiamonds(diamonds);
            _text.text = $"+ {diamonds.ToString("F0")} Coins";
        }

        //reduce count, if count is done exit, else activate new package
        if (rewardsCount <= 0)
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
        itemsData.Save();
        FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
    }

    void ActivatePackage()
    {
        card = null;
        item = null;
        coins = 0;
        diamonds = 0;

        if (!rewards[0].isLootBoxReward)
        {
            index = Random.Range(0, rewards.Count);
            while (rewards[index].chance < Random.value)
            {
                index = Random.Range(0, rewards.Count);
            }

            LootboxReward _rwd = rewards[index];
            if (_rwd.card != null)
            {
                card = _rwd.card;

                cardShow.SetActive(false);
                itemShow.SetActive(false);
                coinsShow.SetActive(false);
                diamondsShow.SetActive(false);
                panel.SetActive(false);
                GameManager.instance.ShowHeroCardAnimation(card);
                LeanTween.delayedCall(7f, () =>
                {
                    collectButton.SetActive(true);
                });

                cardFrame.sprite = frames[(int)card.rarity];
                cardElement.sprite = elements[(int)card.type];
                cardPicture.sprite = card.sprite;
                textCardName.text = card.Name;
                textCardTier.text = card.rarity.ToString();
                textCardHealth.text = $"{card.maxHealth.ToString("F0")}";
                textCardAttack.text = $"{card.attack.ToString("F0")}";
                textCardDefense.text = $"{card.defense.ToString("F0")}";
                textCardDescription.text = card.heroClass.description; https://www.youtube.com/watch?v=fUvA3X-57bs
                return;
            }
            if (_rwd.item != null)
            {
                int amount = Random.Range(_rwd.minAmount, _rwd.maxAmount);
                for (int i = 0; i < amount; i++)
                {
                    //TO DO: Select a random Item from AllItems array in the ItemsData
                    item = _rwd.item;
                }

                cardShow.SetActive(false);
                itemShow.SetActive(true);
                coinsShow.SetActive(false);
                diamondsShow.SetActive(false);
                panel.SetActive(true);
                collectButton.SetActive(true);

                itemPicture.sprite = item.item.sprite;
                textItemName.text = item.item.Name;
                textItemDescription.text = item.item.effect.description;
                return;
            }
            if (_rwd.coinsRange.y > 0)
            {
                coins = Random.Range(_rwd.coinsRange.x, _rwd.coinsRange.y);

                cardShow.SetActive(false);
                itemShow.SetActive(false);
                coinsShow.SetActive(true);
                panel.SetActive(true);
                diamondsShow.SetActive(false);
                collectButton.SetActive(true);
                textCoinsAmount.text = $"+{coins.ToString("F0")}";
                return;
            }
            if (_rwd.diamondsRange.y > 0)
            {
                diamonds = Random.Range(_rwd.diamondsRange.x, _rwd.diamondsRange.y);

                cardShow.SetActive(false);
                itemShow.SetActive(false);
                coinsShow.SetActive(false);
                panel.SetActive(true);
                diamondsShow.SetActive(true);
                collectButton.SetActive(true);
                textDiamondsAmount.text = $"+{diamonds.ToString("F0")}";
                return;
            }
        }
        else
        {
            switch (lootboxIndex)
            {
                case 0:
                    coins = rewards[0].coinsAmount;
                    Debug.Log("Coins Reward: " + lootboxIndex);
                    cardShow.SetActive(false);
                    itemShow.SetActive(false);
                    coinsShow.SetActive(true);
                    panel.SetActive(true);
                    collectButton.SetActive(true);
                    diamondsShow.SetActive(false);
                    textCoinsAmount.text = $"+{coins.ToString("F0")}";
                    lootboxIndex++;
                    return;
                case 1:
                    item = itemsData.GetRandomItem();

                    Debug.Log("Item Reward: " + lootboxIndex + "ItemsLeft: " + totalItemsAmount);
                    cardShow.SetActive(false);
                    itemShow.SetActive(true);
                    coinsShow.SetActive(false);
                    diamondsShow.SetActive(false);
                    collectButton.SetActive(true);
                    panel.SetActive(true);

                    itemPicture.sprite = item.item.sprite;
                    textItemName.text = item.item.Name;
                    textItemDescription.text = item.item.effect.description;
                    totalItemsAmount--;

                    if (totalItemsAmount <= 0)
                    {
                        hasClaimedAllItems = true;
                        lootboxIndex++;
                        return;
                    }
                    else
                    {
                        //ActivatePackage();
                        return;
                    }
                case 2:
                    if (!hasClaimedAllHeroes && totalHeroesAmount == 0)
                    {
                        totalHeroesAmount = rewards[0].heroSummonAmount;
                    }
                    item = itemsData.GetRandomHeroSummon();

                    cardShow.SetActive(false);
                    itemShow.SetActive(true);
                    coinsShow.SetActive(false);
                    diamondsShow.SetActive(false);
                    collectButton.SetActive(true);
                    panel.SetActive(true);

                    itemPicture.sprite = item.item.sprite;
                    textItemName.text = item.item.Name;
                    textItemDescription.text = item.item.effect.description;
                    totalHeroesAmount--;
                    if (totalHeroesAmount <= 0)
                    {
                        hasClaimedAllHeroes = true;
                        lootboxIndex++;
                        return;
                    }
                    else
                    {
                        //ActivatePackage();
                        return;
                    }
                case 3:
                    card = heroBase.GetRandomCard(rewards[0].heroRarity);
                    if (card == null) ActivatePackage();

                    cardShow.SetActive(false);
                    itemShow.SetActive(false);
                    coinsShow.SetActive(false);
                    diamondsShow.SetActive(false);
                    panel.SetActive(false);
                    GameManager.instance.ShowHeroCardAnimation(card);
                    LeanTween.delayedCall(7f, () =>
{
    collectButton.SetActive(true);
});

                    cardFrame.sprite = frames[(int)card.rarity];
                    cardElement.sprite = elements[(int)card.type];
                    cardPicture.sprite = card.sprite;
                    textCardName.text = card.Name;
                    textCardTier.text = card.rarity.ToString();
                    textCardHealth.text = $"{card.maxHealth.ToString("F0")}";
                    textCardAttack.text = $"{card.attack.ToString("F0")}";
                    textCardDefense.text = $"{card.defense.ToString("F0")}";
                    textCardDescription.text = card.heroClass.description; https://www.youtube.com/watch?v=fUvA3X-57bs
                    lootboxIndex++;
                    return;

                default:
                    return;
            }
        }


    }

    public void ShowCard()
    {

    }

    public void CollectingDone()
    {
        //manager.SetState(GameState.Items);
        Destroy(gameObject);
    }

    public void Setup(ItemEffect _box, GameManager _mng)
    {
        manager = _mng;
        EffectBoxReward _efb = _box as EffectBoxReward;

        if (!_efb.isLootBox)
        {
            rewardsCount = Random.Range(1, _efb.maxRewards);

            rewards = new List<LootboxReward>(_efb.reward.Length);
            for (int i = 0; i < _efb.reward.Length; i++)
            {
                rewards.Add(_efb.reward[i]);
            }
        }
        else
        {
            _efb.reward[0].isLootBoxReward = true;
            rewardsCount = 0;
            rewards = new List<LootboxReward>();
            if (_efb.reward[0].coinsAmount > 0)
            {
                rewardsCount++;
                rewards.Add(_efb.reward[0]);
            }
            if (_efb.reward[0].minMaxItemsAmount.x >= 1)
            {
                Debug.Log("has Items: " + _efb.reward[0].minMaxItemsAmount.x + "/" + _efb.reward[0].minMaxItemsAmount.y);
                totalItemsAmount = Mathf.RoundToInt(Random.Range(rewards[0].minMaxItemsAmount.x, rewards[0].minMaxItemsAmount.y));
                rewardsCount += totalItemsAmount;
                // if (!hasClaimedAllItems && totalItemsAmount == 0)
                // {
                // }
            }
            if (_efb.reward[0].heroSummonAmount > 0)
            {
                totalHeroesAmount = _efb.reward[0].heroSummonAmount;
                Debug.Log("has Heroes: " + _efb.reward[0].heroSummonAmount);
                rewardsCount += totalHeroesAmount;
            }

            //This extra add for rewards count is for the hero guaranted for each chest
            rewardsCount++;
        }



    }

}
