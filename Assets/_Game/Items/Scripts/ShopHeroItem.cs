using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Shop Hero Item")]
public class ShopHeroItem : ShopItem {

    public string Name;
    public float costCoins;
    public float costDiamonds;
    public HeroCard[] card;
    int select;

    public void Randomize() {
        select = Random.Range(0, card.Length);
    }
    public HeroCard GetCard() {
        return card[select];
    }
}
