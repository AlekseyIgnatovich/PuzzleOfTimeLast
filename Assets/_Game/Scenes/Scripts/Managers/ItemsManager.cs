using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemsManager : Manager
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
    float gridHeight;

    List<ItemSlot> currentSlots;

    private void OnEnable()
    {
        itemsData.OnShowItems += ShowItems;
        GameManager.onHeroCardUnlocked += UpdateItemsShown;
    }
    private void OnDisable()
    {
        itemsData.OnShowItems -= ShowItems;
        GameManager.onHeroCardUnlocked -= UpdateItemsShown;
    }

    private void Start()
    {
        gridHeight = contentG.GetComponent<RectTransform>().sizeDelta.y;
        itemsData.itemsShow = ItemCase.Game;
        CreateItems();
        ShowItems();
    }

    public void UpdateItemsShown(HeroCard _card = null)
    {
        Debug.Log("Updating items!");
        gridHeight = contentS.GetComponent<RectTransform>().sizeDelta.y;
        itemsData.itemsShow = ItemCase.Story;
        CreateItems();
        ShowItems();
    }

    void CreateItems()
    {
        int _countG = 0;
        int _countS = 0;

        //create items
        ItemObject[] _items = new ItemObject[itemsData.items.Count];
        if (currentSlots != null)
        {
            Debug.Log("Erasing");
            for (int i = 0; i < currentSlots.Count; i++)
            {
                Destroy(currentSlots[i].gameObject);
            }
            currentSlots.Clear();
        }
        else
        {
            currentSlots = new List<ItemSlot>();
        }

        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = itemsData.items[i];
            if (_items[i] != null && _items[i].amount > 0)
            {
                switch (_items[i].itemCase)
                {
                    case ItemCase.Game:
                        ItemSlot _itemslotG = Instantiate(itemSlotG, contentG);
                        _itemslotG.Setup(_items[i], i);
                        currentSlots.Add(_itemslotG);
                        _countG++;
                        break;
                    case ItemCase.Story:
                        ItemSlot _itemSlotS = Instantiate(itemSlotS, contentS);
                        _itemSlotS.Setup(_items[i], i); _countS++;
                        currentSlots.Add(_itemSlotS);
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
        //CreateItems();
        int _index = (int)itemsData.itemsShow;
        for (int i = 0; i < stash.Length; i++)
        {
            stash[i].SetActive(_index == i);
        }
    }

    public void GoToMainMenu()
    {
        gameManager.SetState(GameState.MainMenu);
    }

}
