using UnityEngine;

[CreateAssetMenu(menuName = "My File/System/Rewards/Currency Package")]
public class RewardCurrency : RewardPackage
{

    public float minimumAmount;
    public float maximumAmount;
    public Sprite iconUI;
    float rewardAmount;

    public override void RandomiseChoise()
    {
        rewardAmount = Mathf.Floor(Random.Range(minimumAmount, maximumAmount));
    }

    public override float GetAmount()
    {
        return rewardAmount;
    }

    public override Sprite GetIconSprite()
    {
        return base.GetIconSprite();
    }
}