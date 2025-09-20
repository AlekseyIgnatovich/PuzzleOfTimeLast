using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

    [SerializeField] ItemsData data;
    [Space]
    [SerializeField] Image image;
    public int index;

    public void Setup(int _index) {
        index = _index;
        image.sprite = data.items[index].item.sprite;
    }
}
