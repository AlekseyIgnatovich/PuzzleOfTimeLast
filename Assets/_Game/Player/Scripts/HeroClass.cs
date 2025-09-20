using UnityEngine;

public abstract class HeroClass : ScriptableObject {

    public Color color;
    [Space]
    public string Name = "None";
    [Space]
    [TextArea(2, 4)]
    public string description = "Unknown";
    [Space]
    public Rarity rarity;
    [Space]
    public HeroSpecial special;
    [Space]
    public float attackModifier;
    public float defenseModifier;
    public float healthModifier;
    public float specialModifier;
}
