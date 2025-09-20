using UnityEngine;
using TMPro;

public class GemPopup : MonoBehaviour
{

    [SerializeField] ItemsData itemsData;
    [SerializeField] ItemUse itemUse;
    [SerializeField] Transform itemsGrid;
    [SerializeField] GameObject itemUsePanel;
    [Space]
    [SerializeField] TextMeshProUGUI textItemName;
    [SerializeField] TextMeshProUGUI textItemDescription;
    [SerializeField] TextMeshProUGUI textItemAmount;
    GameplayManager manager;
    Gem gem;

    ItemUse item;

    public void SetPopup(Gem _gem, GameplayManager _manager)
    {
        print("created a gem popup");
        manager = _manager;
        gem = _gem;

        itemUsePanel.SetActive(false);
        CreateItems();
    }

    void CreateItems()
    {
        ItemObject[] _items = new ItemObject[itemsData.items.Count];
        ItemUse _itmuse;
        int _count = 0;
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = itemsData.items[i];
            if ((_items[i] != null) && (_items[i].selected))
            {
                bool _ok = false;
                switch (_items[i].item.targetType)
                {
                    case ItemType.Gem: _ok = gem != null; break;
                }
                if (_ok)
                {
                    _itmuse = Instantiate(itemUse, itemsGrid);
                    _itmuse.Setup(i);
                    _count++;
                }
            }
        }
    }

    public void ApplyItem()
    {
        if (item == null) { return; }
        Item _item = itemsData.items[item.index].item;
        if (_item == null) { return; }

        print($"used {_item.Name} item");
        _item.effect.ApplyEffect(manager, null, null, gem);
        if (!itemsData.items[item.index].SubtractAmount())
        {
            itemsData.items[item.index].selected = false;
            itemsData.items[item.index] = null;
        }
        Kill();
    }

    public void SelectItem(ItemUse _item)
    {
        ItemUse[] _itms = itemsGrid.GetComponentsInChildren<ItemUse>();
        for (int i = 0; i < _itms.Length; i++)
        {
            _itms[i].Unselect();
        }
        item = _item;
        textItemName.text = $"{itemsData.items[item.index].item.Name}";
        textItemDescription.text = $"{itemsData.items[item.index].item.effect.description}";
        textItemAmount.text = $"{itemsData.items[item.index].amount}";
        itemUsePanel.SetActive(true);
    }
    public void HideItemPanel()
    {
        itemUsePanel.SetActive(false);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
