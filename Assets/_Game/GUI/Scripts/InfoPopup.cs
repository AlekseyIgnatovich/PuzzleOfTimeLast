using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPopup : MonoBehaviour
{

    [SerializeField] GameSettings settings;
    [SerializeField] ItemsData itemsData;
    [SerializeField] GameObject itemsUI;
    [SerializeField] ItemUse itemUse;
    [SerializeField] Transform itemsGrid;
    [SerializeField] GameObject buttonLevelup;
    [SerializeField] GameObject buttonItems;
    [SerializeField] GameObject itemUsePanel;
    [Space]
    [SerializeField] Sprite[] frames;
    [SerializeField] Sprite[] elements;
    [Space]
    [SerializeField] Image frame;
    [SerializeField] Image picture;
    [SerializeField] Image element;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textLevel;
    [SerializeField] TextMeshProUGUI textTier;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] TextMeshProUGUI textAttack;
    [SerializeField] TextMeshProUGUI textDefense;
    [SerializeField] TextMeshProUGUI textClassDescription;
    [Space]
    [SerializeField] TextMeshProUGUI textItemName;
    [SerializeField] TextMeshProUGUI textItemDescription;
    [SerializeField] TextMeshProUGUI textItemAmount;

    GameplayManager gameplayManager;
    InventoryManager inventoryManager;
    Hero hero;
    Mob mob;

    ItemUse item;


    public void SetPopup(Hero _hero, Mob _mob, GameplayManager _manager)
    {
        if (_hero != null)
        {
            frame.sprite = frames[(int)_hero.rarity];
            picture.sprite = _hero.card.sprite;
            textName.text = _hero.card.Name;
            textTier.text = _hero.rarity.ToString();
            textHealth.text = $"{_hero.health.ToString("F0")} /\n{_hero.maxHealth.ToString("F0")}";
            textAttack.text = $"{_hero.attack.ToString("F0")}";
            textDefense.text = $"{_hero.defense.ToString("F0")}";
            textLevel.text = (_hero.level + 1).ToString();

            textClassDescription.text = _hero.card.heroClass.description;

            element.sprite = elements[(int)_hero.type];
        }
        else
        {
            picture.sprite = _mob.card.sprite;
            textName.text = _mob.card.Name;
            textTier.text = "";
            textHealth.text = $"{_mob.health.ToString("F0")} /\n{_mob.maxHealth.ToString("F0")}";
            textAttack.text = $"{_mob.attack.ToString("F0")}";
            textDefense.text = $"{_mob.defense.ToString("F0")}";
            textLevel.text = $"{_mob.level}";

            textClassDescription.text = "";
        }

        gameplayManager = _manager;
        hero = _hero;
        mob = _mob;
        CreateItems();
        itemsUI.SetActive(false);
        buttonLevelup.SetActive(false);
        buttonItems.SetActive(true);
        itemUsePanel.SetActive(false);
    }

    public void SetPopup(HeroItem _hero, HeroCard _card, InventoryManager _manager)
    {
        if (_hero != null)
        {
            frame.sprite = frames[(int)_hero.rarity];
            picture.sprite = _card.sprite;
            textName.text = _card.Name;
            textTier.text = _hero.rarity.ToString();
            textHealth.text = $"{_hero.maxHealth.ToString("F0")}";
            textAttack.text = $"{_hero.attack.ToString("F0")}";
            textDefense.text = $"{_hero.defense.ToString("F0")}";
            textLevel.text = (_hero.level + 1).ToString();

            float _xpt = settings.xpThreshold.roof[_hero.level];

            textClassDescription.text = _card.heroClass.description;

            element.sprite = elements[(int)_hero.type];
        }
        itemsUI.SetActive(false);
        buttonLevelup.SetActive(true);
        buttonItems.SetActive(false);
        itemUsePanel.SetActive(false);

        inventoryManager = _manager;
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
                    case ItemType.Hero: _ok = hero != null; break;
                    case ItemType.Enemy: _ok = mob != null; break;
                }
                if (_ok)
                {
                    _itmuse = Instantiate(itemUse, itemsGrid);
                    _itmuse.Setup(i);
                    _count++;
                }
            }
        }

        if (_count > 0)
        {
            buttonItems.SetActive(true);
            return;
        }
        buttonItems.SetActive(false);
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

    public void ApplyItem()
    {
        Item _item = itemsData.items[item.index].item;
        print($"used {_item.Name} item");
        _item.effect.ApplyEffect(gameplayManager, hero, mob, null);
        if (!itemsData.items[item.index].SubtractAmount())
        {
            itemsData.items[item.index].selected = false;
            itemsData.items[item.index] = null;
        }
        Kill();
    }

    public void LevelUp()
    {
        inventoryManager.gameManager.SetState(GameState.LevelUp);
        Kill();
    }
    public void ShowItems()
    {
        itemsUI.SetActive(true);
        buttonItems.SetActive(false);
    }
    public void HideItems()
    {
        itemsUI.SetActive(false);
        buttonItems.SetActive(true);
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
