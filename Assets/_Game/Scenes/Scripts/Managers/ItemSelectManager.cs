using UnityEngine;

public class ItemSelectManager : Manager
{

    public ItemsData itemsData;
    [Space]
    [SerializeField] GameObject[] stash;
    [Space]
    [SerializeField] Transform contentG;
    [SerializeField] Transform contentS;
    [Space]
    [SerializeField] ItemSlot itemSlotG;
    [SerializeField] ItemSlot itemSlotS;
    [Space]
    [SerializeField] float extraHeight;
    [SerializeField] GameObject instruction;
    float gridHeight;

    public override void Initialize(GameManager _manager)
    {
        gameManager = _manager;
        canvas.worldCamera = Camera.main;


        gridHeight = contentG.GetComponent<RectTransform>().sizeDelta.y;
        itemsData.itemsShow = ItemCase.Game;
        CreateItems();
        ShowItems();
        if (gameManager.isFirstTime)
        {
            instruction.SetActive(true);
        }
    }

    void CreateItems()
    {
        int _countG = 0;
        int _countS = 0;

        //create items
        ItemObject[] _items = new ItemObject[itemsData.items.Count];
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = itemsData.items[i];
            if (_items[i] != null)
            {
                switch (_items[i].itemCase)
                {
                    case ItemCase.Game:
                        if (_items[i].item.targetType != ItemType.Inventory)
                        {
                            Instantiate(itemSlotG, contentG).Setup(_items[i], i); _countG++;
                        }
                        break;
                    case ItemCase.Story:
                        if (_items[i].item.targetType != ItemType.Inventory)
                        {
                            Instantiate(itemSlotS, contentS).Setup(_items[i], i); _countS++;
                        }
                        break;
                }
            }
        }

        //game items size
        RectTransform _folder = contentG.GetComponent<RectTransform>();
        Vector2 _size = _folder.sizeDelta;
        _size.y = gridHeight + (((_countG - 1) / 3) * gridHeight) + extraHeight;
        _folder.sizeDelta = _size;
        //story items size
        _folder = contentS.GetComponent<RectTransform>();
        _size = _folder.sizeDelta;
        _size.y = gridHeight + (((_countG - 1) / 3) * gridHeight) + extraHeight;
        _folder.sizeDelta = _size;
    }

    public void ShowItems()
    {
        int _index = (int)itemsData.itemsShow;
        for (int i = 0; i < stash.Length; i++)
        {
            stash[i].SetActive(_index == i);
        }
    }

}
