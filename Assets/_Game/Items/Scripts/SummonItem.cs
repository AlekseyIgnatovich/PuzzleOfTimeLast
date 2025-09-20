using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Summon Item")]
public class SummonItem : ScriptableObject {

    public string Name;
    public Sprite image;
    [Space]
    public HeroCard[] cards;
    [Space]
    public int amount;
    public float costUSD;

}
