using UnityEngine;

public class HeroSelectManager : Manager
{

    public HeroBase heroBase;
    [Space]
    [SerializeField] HeroSlot slotItem;
    [SerializeField] Transform storedFolder;
    [SerializeField] float extraHeight;
    [SerializeField] GameObject instruction;
    float gridHeight;

    public override void Initialize(GameManager _manager)
    {
        gameManager = _manager;
        canvas.worldCamera = Camera.main;

        gridHeight = storedFolder.GetComponent<RectTransform>().sizeDelta.y;

        CreateCards();
        if (gameManager.isFirstTime)
        {
            instruction.SetActive(true);
        }
    }

    void CreateCards()
    {
        int _cards = 0;

        HeroItem _heroitem;
        for (int i = 0; i < heroBase.hero.Length; i++)
        {
            _heroitem = heroBase.hero[i];
            if (_heroitem != null)
            {
                _cards++;
                Instantiate(slotItem, storedFolder).SetData(i);
            }
        }

        RectTransform _folder = storedFolder.GetComponent<RectTransform>();
        Vector2 _size = _folder.sizeDelta;
        _size.y = gridHeight + (((_cards - 1) / 3) * gridHeight) + extraHeight;
        _folder.sizeDelta = _size;

        ArrangeToPower();
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
}
