using UnityEngine;

[CreateAssetMenu(menuName = "My File/System/Rewards/Item Package")]
public class RewardItem : RewardPackage {

    public ItemObject[] items;
    int rewardIndex;

    public override void RandomiseChoise() {
        rewardIndex = Random.Range(0, items.Length);
    }

    public override ItemObject GetItem() {
        return items[rewardIndex];
    }

    public override void CollectContent() {

    }
}
