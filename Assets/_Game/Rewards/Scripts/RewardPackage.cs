using UnityEngine;

public abstract class RewardPackage : ScriptableObject
{

    public RewardType type;
    public Sprite icon;
    public int amount;
    public string description;

    public virtual void RandomiseChoise()
    {

    }

    public virtual HeroCard GetCard()
    {
        return null;
    }
    public virtual ItemObject GetItem()
    {
        return null;
    }
    public virtual float GetAmount()
    {
        return 0;
    }

    public virtual void CollectContent()
    {
    }

    public virtual Sprite GetIconSprite()
    {
        return null;
    }


}
