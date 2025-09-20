using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackItem : MonoBehaviour {

    [SerializeField] BuyItemPopupNew buyPopup;
    [Space]
    public ItemObject item;
    public CurrencyItem currencyItem;
    public int count;
    public Currency currency;
    public float cost;
    [Space]
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    private void Start() {
        UpdateItem();
    }

    public void UpdateItem() {
        if (item != null) {
            image.sprite = item.item.sprite;
            if (text != null) { text.text = $"x{count}"; }
            return;
        }
        if (currencyItem != null) {
            image.sprite = currencyItem.image;
            if (text != null) { text.text = $"x{count}"; }
            return;
        }
    }

    public string GetName() {
        if (item != null) {
            return item.item.Name;
        }
        if (currencyItem != null) {
            return currencyItem.Name;
        }
        return "";
    }
    public string GetDescription() {
        if (item != null) {
            return item.item.effect.description;
        }
        if (currencyItem != null) {
            return currencyItem.Name;
        }
        return "";
    }

    public void BuyPopup() {
        if (buyPopup == null) { return; }

        buyPopup.Setup(null, this);
    }
    public void BuyVideo() {

    }
}
