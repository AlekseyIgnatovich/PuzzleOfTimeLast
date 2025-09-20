using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Lottery Box")]
public class LotteryBox : ScriptableObject {

    public string Name;
    [Space]
    public Sprite sprite;
    [Space]
    [TextArea(2, 4)]
    public string description;
    [Space]
    public float coinsCost;
    public float diamondsCost;
    public AdsVideo videoCheck;
    [Space]
    public HeroCard[] cards;

    int rewardIndex;

    public void RandomiseChoise() {
        rewardIndex = Random.Range(0, cards.Length);
    }

    public HeroCard GetCard() {
        return cards[rewardIndex];
    }
}
