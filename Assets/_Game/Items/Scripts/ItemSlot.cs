using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{

    [SerializeField] ItemsData data;
    [Space]
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textCount;
    [SerializeField] GameObject battleMark;
    [Space]
    public ItemObject itemObject;
    [Space]
    public bool forbattle;
    public int index;//index in inventory

    private void Start()
    {
        UpdateItem();
    }

    public void Setup(ItemObject _obj, int _index)
    {
        itemObject = _obj;
        index = _index;

        forbattle = data.items[index].selected;
        battleMark.SetActive(forbattle);
    }

    void UpdateItem()
    {
        if (itemObject == null || itemObject.amount == 0)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            image.sprite = itemObject.item.sprite;
            textName.text = itemObject.item.Name;
            textCount.text = $"x{itemObject.amount}";
        }
    }

    public void SelectForBattle()
    {
        if (!data.SelectForBattle(index, forbattle))
        {
            forbattle = false;
            battleMark.SetActive(forbattle);
        }
    }
    public void ToggleForBattle()
    {
        forbattle = !forbattle;
        battleMark.SetActive(forbattle);
    }

    public void SelectItem()
    {
        data.SelectItem(itemObject);
    }
}
