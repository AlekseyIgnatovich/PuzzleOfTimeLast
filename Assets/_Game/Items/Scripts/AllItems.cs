using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/All Items")]
public class AllItems : ScriptableObject {

    [Header("All items availible in the game")]
    public ItemObject[] items;

    public void ClearAllItems() {
        for (int i = 0; i < items.Length; i++) {
            items[i].selected = false;
            items[i].amount = 0;
        }
    }
}
