using UnityEngine;

[CreateAssetMenu(menuName = "My File/System/Rewards/Hero Card Package")]
public class RewardHeroCard : RewardPackage
{

    public HeroCard[] cards;
    int rewardIndex;

    public override void RandomiseChoise()
    {
        rewardIndex = Random.Range(0, cards.Length);
    }

    public override HeroCard GetCard()
    {
        if (cards == null) { return null; }
        return cards[rewardIndex];
    }

    public override void CollectContent()
    {

    }
}
