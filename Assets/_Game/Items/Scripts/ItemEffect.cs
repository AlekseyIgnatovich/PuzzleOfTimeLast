using UnityEngine;

public abstract class ItemEffect : ScriptableObject {

    public ItemEntity entity;
    [TextArea(2, 4)]
    public string description = "Unknown";

    public virtual bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem) {
        Debug.Log($"Hero Target: {_herotarg.card.Name}");
        Debug.Log($"Mob Target: {_mobtarg.card.Name}");
        Debug.Log($"Gem Target: {_gem.Type}");
        return false;
    }
}
