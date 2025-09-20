using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Currency Item")]
public class CurrencyItem : ScriptableObject {

    public string Name;
    public Sprite image;
    [Space]
    public Currency currency;
    public float amount;
    public float costUSD;

}
