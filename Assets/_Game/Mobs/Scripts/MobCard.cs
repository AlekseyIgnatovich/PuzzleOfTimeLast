using UnityEngine;

[CreateAssetMenu(menuName = "My File/Mobs/Mob Card")]
public class MobCard : ScriptableObject
{

    public string Name;
    [Space]
    public float maxHealth;
    public float health;
    [Space]
    public int attack;
    public int defense;
    public int turnsCount;
    [Space]
    public MobAbility[] abilities;
    [Space]
    public float value;
    [Space]
    public bool isHero;
    public RuntimeAnimatorController animator;
    public Vector2 size;
    [Space]
    public AudioClip attackSFX;
    public AudioClip spawnSFX;
    public AudioClip deathSFX;
    public Sprite sprite;


    // This section is for selecting the color of the enemy
    public bool isCustom;
    public ElementType type;

    public Resistance[] resistances;

}
