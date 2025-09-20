using UnityEngine;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour {

    [SerializeField] ItemsData itemData;
    [SerializeField] Image image;
    public int index;

    public void Setup(int _index) {
        index = _index;
        image.sprite = itemData.items[index].item.sprite;
    }

    public void Pick() {

    }
    public void Select() {
    }
    public void Unselect() {
    }
}
