using UnityEngine;

[System.Serializable]
public class LootboxReward
{

    [Range(0, 1)]
    public float chance;
    [Space]
    public int minAmount;
    public int maxAmount;
    public Vector2 coinsRange;
    public Vector2 diamondsRange;
    public ItemObject item;
    public HeroCard card;
    //TO DO: DISABLE ALL ABOVE
    [Space(10)]
    [Header("NEW VARIABLES")]
    [Space(10)]

    public bool isLootBoxReward;
    public float coinsAmount;
    public float diamondsAmount;
    public Vector2 minMaxItemsAmount;
    public int heroSummonAmount;
    public Rarity heroRarity;


}
