using UnityEngine;

[CreateAssetMenu(menuName ="My File/Heroes/Hero Card")]
public class HeroCard : ScriptableObject {

    public string Name;
    [Space]
    public float maxHealth;
    public float health;
    public float attack;
    public float defense;
    public ElementType type;
    [Space]
    public float specialMaxPoints;
    [Range(0, 5f)]
    public float specialPercent = 1f;
    [Space]
    public Rarity rarity;
    public HeroClass heroClass;
    [Space]
    public Sprite sprite;
    public Sprite cardSprite;
    public Sprite gameSprite;
    [Space]
    public AudioClip attackSFX;
    public AudioClip attackSpecialSFX;
    public AudioClip spawnSFX;
    public AudioClip deathSFX;

    public float GetPower(int _level) {
        float _attack = attack + (heroClass.attackModifier * _level);
        float _defense = defense + (heroClass.defenseModifier * _level);
        float _maxHealth = maxHealth + (heroClass.healthModifier * _level);
        return Mathf.Floor((_attack + _defense + _maxHealth) / 3);
    }
}
