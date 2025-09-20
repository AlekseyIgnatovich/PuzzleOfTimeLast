using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Diamonds Item")]
public class DiamondsItem : ScriptableObject {

    public string Name;
    public Sprite image;
    [Space]
    public float diamonds;
    public float costUSD;

}
