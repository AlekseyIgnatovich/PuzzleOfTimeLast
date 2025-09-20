using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "My File/Items/Items Data")]
public class ItemsData : ScriptableObject
{

    [SerializeField] GameSettings settings;
    [Space]
    [SerializeField] AllItems allItems;
    [Space]
    [Header("--DEFAULT ITEMS--")]
    [SerializeField] ItemDefault[] itemsDefault;
    [Space]
    [Header("--PLAY ITEMS--")]
    public List<ItemObject> items;
    public ItemObject[] heroSummons;
    public ItemObject[] consumables;

    public ItemObject selectedItem;
    public ItemCase itemsShow;
    public Currency selectedCurrency;
    public int shopShow;
    [Space]
    public int maxItems;
    [Space]
    public ShopHeroItem heroUnlockPack;

    public event Action OnSelectItem;
    public event Action OnShowItems;
    public event Action OnShowShop;
    public event Action OnSwitchCurrency;

    public delegate void OnItemAdded();
    public static OnItemAdded onItemAdded;

    public void AddItem(ItemObject _item)
    {
        bool _done = false;
        //if item is currency

        //check for same item 
        for (int i = 0; i < items.Count; i++)
        {
            //Debug.Log("Comparing item: " + _item.id + "with: " + items[i].id);
            if (items[i] == _item)
            {
                items[i].amount++;
                _done = true;
                //Debug.Log("Se encontro item. Amount: " + items[i].amount);
                break;
            }
        }
        //if not done check for null item
        if (!_done)
        {
            bool hasSpace = false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    items[i] = _item;
                    items[i].amount = 1;
                    hasSpace = true;
                    break;
                }
            }

            if (!hasSpace)
            {
                items.Add(_item);
                items[items.Count - 1].amount = 1;
                Debug.Log("Adding new item: " + _item);
            }
        }
        Save();
    }
    public void Save()
    {
        // BinaryFormatter formatter = new BinaryFormatter();
        // string path = Application.persistentDataPath + "/items.sav";
        // FileStream stream = new FileStream(path, FileMode.Create);
        EraseDuplicated();

        SaveItem[] _stash = new SaveItem[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            _stash[i] = new SaveItem();
            if (items[i] == null)
            {
                //items.RemoveAt(i);
                _stash[i].itemID = "";
                _stash[i].itemAmount = 0;
                continue;
            }
            else
            {
                _stash[i].itemID = items[i].id;
                _stash[i].itemAmount = items[i].amount;
            }
        }

        FirebaseManager.instance.firestoreManager.UpdateSaveItems(_stash);
        FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
        // formatter.Serialize(stream, _stash);

        // stream.Close();

        Debug.Log($"Saved items data to file");
    }

    void EraseDuplicated()
    {
        for (int i = 0; i < items.Count; i++)
        {
            ItemObject item = items[i];
            if (item == null)
            {
                continue;
            }
            if (i == items.Count - 1) return;
            for (int j = i + 1; j < items.Count; j++)
            {
                if (items[j] == null) continue;
                if (items[j].id == item.id)
                {
                    items.RemoveAt(j);
                    continue;
                }
            }
        }
    }

    public void Load(string[] _itemsID, int[] _itemsAmount)
    {
        items = new List<ItemObject>();
        for (int i = 0; i < _itemsID.Length; i++)
        {
            items.Add(GetItemById(_itemsID[i]));
            Debug.Log("Loading item: " + _itemsID[i] + "/" + _itemsAmount[i]);
            Debug.Log("El items[i]: " + items[i]);
            if (items[i] != null)
            {
                if (_itemsAmount[i] == 0)
                {
                    Debug.Log("No more in invetory : " + items[i].name);
                }
                else
                {
                    items[i].amount = _itemsAmount[i];
                    items[i].selected = false;
                }
            }
        }
        EraseDuplicated();
        // string path = Application.persistentDataPath + "/items.sav";
        // int sv = PlayerPrefs.GetInt("saveversion", 0);

        // if (File.Exists(path) && (sv == settings.saveVersion))
        // {
        //     BinaryFormatter formatter = new BinaryFormatter();
        //     FileStream stream = new FileStream(path, FileMode.Open);

        //     // SaveItem[] _stash = formatter.Deserialize(stream) as SaveItem[];

        //     for (int i = 0; i < items.Length; i++)
        //     {
        //         items[i] = GetItemById(saveItems[i].itemID);
        //         if (items[i] != null)
        //         {
        //             items[i].amount = saveItems[i].itemAmount;
        //             items[i].selected = false;
        //         }
        //     }

        //     stream.Close();
        //     Debug.Log($"Loaded items from file");
        // }
        // else
        // {
        //     for (int i = 0; i < items.Length; i++)
        //     {
        //         items[i] = itemsDefault[i].item;
        //         if (items[i] != null)
        //         {
        //             items[i].amount = itemsDefault[i].amount;
        //             items[i].selected = false;
        //         }
        //     }

        //     Debug.Log($"Loaded item defaults");

        //     Save();
        // }
    }
    public SaveItem[] LoadDefaultItems()
    {
        for (int i = 0; i < itemsDefault.Length; i++)
        {
            items[i] = itemsDefault[i].item;
            if (items[i] != null)
            {
                items[i].amount = itemsDefault[i].amount;
                items[i].selected = false;
            }
        }

        SaveItem[] _stash = new SaveItem[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            _stash[i] = new SaveItem();
            if (items[i] == null)
            {
                _stash[i].itemID = "";
                _stash[i].itemAmount = 0;
            }
            else
            {
                _stash[i].itemID = items[i].id;
                _stash[i].itemAmount = items[i].amount;
            }
        }

        return _stash;
    }

    public void DeleteSave()
    {
        string path = Application.persistentDataPath + "/items.sav";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Items save deleted");
        }
    }

    public ItemObject GetItemById(string _id)
    {
        ItemObject[] _items = allItems.items;
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i].id == _id) { return _items[i]; }
        }
        return null;
    }

    public void SelectItem(ItemObject _item)
    {
        Debug.Log("Select Item: " + _item.id);
        selectedItem = _item;
        OnSelectItem?.Invoke();
    }
    public int SelectedItemIndex()
    {
        if (selectedItem != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if ((items[i] != null) && (items[i].id == selectedItem.id))
                {
                    return i;
                }
            }
        }
        return -1;
    }
    public void ShowItems(int _index)
    {
        itemsShow = (ItemCase)_index;
        OnShowItems?.Invoke();
    }
    public void ShowShop(int _index)
    {
        shopShow = _index;
        OnShowShop?.Invoke();
    }
    public void SwitchCurrency(int _index)
    {
        selectedCurrency = (Currency)_index;
        OnSwitchCurrency?.Invoke();
    }

    public bool SelectForBattle(int _index, bool _select)
    {

        if (_select)
        {
            int _count = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if ((items[i] != null) && (items[i].selected))
                {
                    _count++;
                    if (_count >= maxItems)
                    {
                        return false;
                    }
                }
            }
        }

        items[_index].selected = _select;

        return true;
    }
    public int GetBattleReadyItems()
    {
        int _count = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if ((items[i] != null) && (items[i].selected))
            {
                _count++;
            }
        }
        return _count;
    }
    public ItemObject GetItemInOrder(int _index)
    {
        int _count = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if ((items[i] != null) && (items[i].selected))
            {
                if (_count == _index)
                {
                    return items[i];
                }
                _count++;
            }
        }
        return null;
    }


    public ItemObject GetRandomItem()
    {
        int _index = UnityEngine.Random.Range(0, consumables.Length);
        ItemObject _item = consumables[_index];
        return _item;
    }

    public ItemObject GetRandomHeroSummon()
    {
        int _index = UnityEngine.Random.Range(0, heroSummons.Length);
        ItemObject _item = heroSummons[_index];
        return _item;
    }
}
