using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemLoad : MonoBehaviour {

    [SerializeField] ItemsData data;
    [Space]
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textCount;

    public void Setup(ItemObject _obj) {
        image.sprite = _obj.item.sprite;
        textName.text = _obj.item.Name;
        textCount.text = $"{_obj.amount}";
    }
}
