using UnityEngine;

[System.Serializable]
public class Item {

    public string Name;
    [Space]
    public ItemEffect effect;
    [Space]
    public ItemType targetType;
    [Space]
    public Sprite sprite;
    [Space]
    public float coinsCost;
    public float diamondsCost;
    public AdsVideo videoCheck;
}
