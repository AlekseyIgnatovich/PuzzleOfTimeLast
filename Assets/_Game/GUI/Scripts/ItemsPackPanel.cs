using UnityEngine;

public class ItemsPackPanel : MonoBehaviour {

    [SerializeField] BuyItemPopupNew buyPopup;
    [Space]
    public string Name;
    public string description;
    public Transform itemsFolder;
    public PackItem[] items;
    [Space]
    public Currency currency;
    public float cost;

    private void Start() {
        GetItems();
    }

    void GetItems() {
        items = itemsFolder.GetComponentsInChildren<PackItem>();
    }

    public void BuyPopup() {
        if (buyPopup == null) { return; }

        buyPopup.Setup(this, null);
    }
}
