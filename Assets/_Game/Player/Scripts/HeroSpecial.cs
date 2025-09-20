using UnityEngine;

public abstract class HeroSpecial : ScriptableObject {

    public string Name = "None";
    [Space]
    public ItemType targetType;
    [Space]
    public Sprite sprite;
    [TextArea(2, 4)]
    public string description = "Unknown";
    [Space]
    public Rarity rarity;

    public virtual bool ApplySpecial(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Hero _hero) {
        Debug.Log($"Hero Target: {_herotarg.card.Name}");
        Debug.Log($"Mob Target: {_mobtarg.card.Name}");
        Debug.Log($"Hero: {_hero.card.Name}");
        return false;
    }
}
