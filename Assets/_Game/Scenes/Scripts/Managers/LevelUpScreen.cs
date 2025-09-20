using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelUpScreen : Manager
{

    [Header("Refrences")]
    [SerializeField] GameSettings settings;
    [SerializeField] HeroBase heroBase;
    [SerializeField] Sprite emptySprite;
    [SerializeField] HeroSlot slotItem;
    [SerializeField] Transform storedFolder;
    [Space]
    [Header("Selected Card")]
    [SerializeField] Image imageHero;
    [SerializeField] Image imageFrame;
    [SerializeField] Image imageLabel;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textLevel;
    [SerializeField] TextMeshProUGUI textExperience;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] TextMeshProUGUI textAttack;
    [SerializeField] TextMeshProUGUI textDefense;
    [SerializeField] TextMeshProUGUI textTier;
    [SerializeField] TextMeshProUGUI textSelected;
    [SerializeField] RectTransform barFill;
    [Space]
    [SerializeField] TextMeshProUGUI textLevelX;
    [SerializeField] RectTransform barFillX;
    [SerializeField] TextMeshProUGUI textHealthX;
    [SerializeField] TextMeshProUGUI textAttackX;
    [SerializeField] TextMeshProUGUI textDefenseX;
    Vector2 barSize;
    [Space]
    [Header("Settings")]
    [SerializeField] float extraHeight;
    [SerializeField] GameObject instruction;

    float gridHeight;

    int vrLevel;
    float vrExperience;

    private void OnEnable()
    {
        heroBase.OnHeroToggle += UpdateNumberOfSelected;
    }
    private void OnDisable()
    {
        heroBase.OnHeroToggle -= UpdateNumberOfSelected;
    }
    private void Awake()
    {
        barSize = barFill.sizeDelta;
    }

    private void Start()
    {
        gridHeight = storedFolder.GetComponent<RectTransform>().sizeDelta.y;

        CreateCards();
        UpdateSelectedCardUI();
        if (GameManager.instance.isFirstLevelUp)
        {
            instruction.SetActive(true);
        }
    }

    public void LevelUpSelectedHero()
    {
        if (gameManager.isFirstLevelUp)
        {
            gameManager.isFirstLevelUp = false;
            PlayerPrefs.SetInt("Is_First_LevelUp", 1);
        }
        HeroSlot[] _slots = storedFolder.GetComponentsInChildren<HeroSlot>();
        int _count = _slots.Length;
        int _fcount = heroBase.hero.Length;

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].sacrifice)
            {
                _count--;
                _fcount--;
                _slots[i].sacrifice = false;

                heroBase.GetSelectedHero().GetExperience(heroBase, heroBase.hero[_slots[i].index]);

                heroBase.hero[_slots[i].index].dead = true;
            }
            Destroy(_slots[i].gameObject);
        }

        HeroItem[] _newhero = new HeroItem[_fcount];
        HeroItem _hs;
        int k = 0;
        for (int j = 0; j < heroBase.hero.Length; j++)
        {
            _hs = heroBase.hero[j];
            if (!_hs.dead)
            {
                if (heroBase.selectedHero == j) { heroBase.selectedHero = k; }
                _newhero[k] = new HeroItem();
                _newhero[k].SetData(_hs.index, _hs.maxHealth, _hs.attack, _hs.defense, _hs.type, _hs.rarity, _hs.level, _hs.experience, _hs.specialMaxPoints, _hs.specialPercent, _hs.selected);
                k++;
            }
        }
        heroBase.hero = _newhero;

        StartCoroutine("CreateCardsTimer");
        UpdateSelectedCardUI();
        //UpdateFolderSize(_count);
    }

    void UpdateSelectedCardUI()
    {
        HeroItem _hitem = heroBase.GetSelectedHero();
        if (_hitem == null)
        {
            imageHero.sprite = emptySprite;
            imageFrame.sprite = heroBase.GetElementFrame(ElementType.None);
            imageLabel.sprite = heroBase.GetElementLabel(ElementType.None);

            textName.text = $"";
            textLevel.text = $"0";
            textExperience.text = $"0/0";
            textHealth.text = $"0";
            textAttack.text = $"0";
            textDefense.text = $"0";

            textTier.text = Rarity.Common.ToString();
        }
        else
        {
            HeroCard _hcard = heroBase.GetSelectedCard();
            float _minxp = _hitem.experience;
            float _maxxp = settings.xpThreshold.GetXPThreshold(_hitem.level);

            imageHero.sprite = _hcard.gameSprite;
            imageFrame.sprite = heroBase.GetElementFrame(_hcard.type);
            imageLabel.sprite = heroBase.GetElementLabel(_hcard.type);

            float _xpLast = Mathf.Max(settings.xpThreshold.GetXPThreshold(_hitem.level - 1), 0);

            textName.text = $"{_hcard.Name}";
            textLevel.text = (_hitem.level + 1).ToString();
            textExperience.text = $"{_hitem.experience - _xpLast}/{settings.xpThreshold.GetXPThreshold(_hitem.level) - _xpLast}";
            textHealth.text = $"{_hitem.maxHealth}";
            textAttack.text = $"{_hitem.attack}";
            textDefense.text = $"{_hitem.defense}";

            textTier.text = _hitem.rarity.ToString();
            print(_hitem.rarity.ToString());

            barFill.sizeDelta = new Vector2(Mathf.Clamp(_minxp / _maxxp, 0, 1) * barSize.x, barSize.y);
        }
        UpdateNumberOfSelected();
    }
    void UpdateNumberOfSelected()
    {
        HeroSlot[] _slots = storedFolder.GetComponentsInChildren<HeroSlot>();
        int _count = 0;
        HeroItem _item;

        float _xp = 0;

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].sacrifice)
            {
                _count++;
                _item = heroBase.hero[_slots[i].index];
                _xp += settings.rarityXP[(int)_item.rarity] + (_item.level * 50);
            }
        }
        print($"xp to get {_xp}");
        GetVRExperience(heroBase.GetSelectedHero(), _xp);

        textSelected.text = $"Heroes selected: {_count}";
    }

    void CreateCards()
    {
        int _count = 0;
        HeroItem _heroitem;
        for (int i = 0; i < heroBase.hero.Length; i++)
        {
            _heroitem = heroBase.hero[i];
            if ((_heroitem != null) && (_heroitem != heroBase.GetSelectedHero()))
            {
                _count++;
                Instantiate(slotItem, storedFolder).SetData(i);
            }
        }
        ArrangeToPower();
        UpdateFolderSize(_count);
    }

    void UpdateFolderSize(int _count)
    {
        RectTransform _folder = storedFolder.GetComponent<RectTransform>();
        Vector2 _size = _folder.sizeDelta;

        _size.y = gridHeight + (((_count - 1) / 3) * gridHeight) + extraHeight;
        _folder.sizeDelta = _size;
    }

    public void GetVRExperience(HeroItem _item, float _amount)
    {
        float _roof;
        float _am;
        ExperienceLevels xpt = settings.xpThreshold;
        int mlv = settings.maxLevels[(int)_item.rarity];

        vrLevel = _item.level;
        vrExperience = _item.experience;

        if (vrLevel < mlv)
        {
            while (_amount > 0)
            {
                _roof = xpt.roof[vrLevel];
                _am = Mathf.Min(_amount, _roof - vrExperience);
                _amount -= _am;
                vrExperience += _am;
                if (vrExperience >= _roof)
                {
                    vrLevel++;
                }
            }
        }
        if (vrLevel >= mlv)
        {
            vrLevel = mlv;
            vrExperience = xpt.roof[vrLevel];
        }
        bool _set = vrLevel != _item.level;
        float _minxp = vrExperience;
        float _maxxp = xpt.GetXPThreshold(vrLevel);

        HeroCard _card = heroBase.heroCards[_item.index];

        textLevelX.gameObject.SetActive(_set);
        textHealthX.gameObject.SetActive(_set);
        textAttackX.gameObject.SetActive(_set);
        textDefenseX.gameObject.SetActive(_set);
        barFillX.gameObject.SetActive(vrExperience != _item.experience);

        textLevelX.text = $"-> {vrLevel + 1}";
        textHealthX.text = $"-> {_card.maxHealth + (_card.heroClass.healthModifier * vrLevel)}";
        textAttackX.text = $"-> {_card.attack + (_card.heroClass.attackModifier * vrLevel)}";
        textDefenseX.text = $"-> {_card.defense + (_card.heroClass.defenseModifier * vrLevel)}";
        barFillX.sizeDelta = new Vector2(Mathf.Clamp(_minxp / _maxxp, 0, 1) * barSize.x, barSize.y);
    }

    void ArrangeToPower()
    {
        if (storedFolder.childCount < 2) { return; }

        HeroSlot _heroP;
        HeroSlot _heroT;
        bool _loopin = true;

        while (_loopin)
        {
            _loopin = false;
            _heroP = storedFolder.GetChild(0).GetComponent<HeroSlot>();
            for (int i = 1; i < storedFolder.childCount; i++)
            {
                _heroT = storedFolder.GetChild(i).GetComponent<HeroSlot>();
                if (_heroT.GetPower() > _heroP.GetPower())
                {
                    _heroT.transform.SetSiblingIndex(i - 1);
                    _loopin = true;
                    break;
                }
                _heroP = _heroT;
            }
        }
    }

    IEnumerator CreateCardsTimer()
    {
        yield return new WaitForSeconds(.05f);
        CreateCards();
    }
}
