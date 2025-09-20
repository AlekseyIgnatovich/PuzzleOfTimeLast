using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Item Object")]
public class ItemObject : ScriptableObject {

    public string id;
    public ItemCase itemCase;
    public int amount;
    [Space]
    public Item item;
    [Space]
    public bool selected;

    public bool SubtractAmount() {
        amount--;
        return amount > 0;
    }
}
