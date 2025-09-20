using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPanel : MonoBehaviour {

    [SerializeField] Image picture;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] TextMeshProUGUI itemAmount;

    public void Setup(ItemObject _item) {
        gameObject.SetActive(true);

        picture.sprite = _item.item.sprite;
        itemName.text = _item.item.Name;
        itemDescription.text = _item.item.effect.description;
        itemAmount.text = $"{_item.amount}";
    }
}
