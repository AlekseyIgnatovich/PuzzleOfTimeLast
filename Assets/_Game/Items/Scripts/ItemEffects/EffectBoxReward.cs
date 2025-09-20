using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Effects/Lootbox Reward")]
public class EffectBoxReward : ItemEffect
{

    public int maxRewards;
    public Sprite icon;
    public bool isLootBox;
    [Header("Rewards")]
    public LootboxReward[] reward;

    public EffectBoxReward()
    {
        description = "Open this Lootbox in your inventory and gain a reward.";
    }

    public override bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem)
    {

        return false;
    }

}
