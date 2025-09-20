using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroUnlockManager : Manager
{

    [SerializeField] ItemsData data;
    [SerializeField] ScrolledHeroButton heroButtonPrefab;
    [SerializeField] ShopHeroButton buyButton;
    [SerializeField] GameObject videoButton;
    [SerializeField] Sprite[] gemSprites;
    [Space]
    [SerializeField] Image cardElement;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI cardHealth;
    [SerializeField] TextMeshProUGUI cardAttack;
    [SerializeField] TextMeshProUGUI cardDefense;
    [SerializeField] TextMeshProUGUI cardTier;
    [Space]
    [SerializeField] TextMeshProUGUI costCoins;
    [SerializeField] TextMeshProUGUI costDiamonds;
    [Space]
    [SerializeField] ScrollRect heroScroller;
    [SerializeField] int minimumCards;
    [Space]
    [SerializeField] float autoMoveSpeed;
    [SerializeField] Vector3 direction;
    [SerializeField] float scrollMulti;

    bool drag;
    [SerializeField] float cardWidth;

    ShopHeroItem heroUnlockPack;
    ScrolledHeroButton[] cards;
    int selected;
    float contentWidth;

    public bool Drag { get { return drag; } set { drag = value; } }

    public override void Initialize(GameManager _manager)
    {
        gameManager = _manager;
        canvas.worldCamera = Camera.main;

        Setup(data.heroUnlockPack);
    }

    private void Update()
    {

        if (!drag) { heroScroller.content.transform.localPosition += direction * autoMoveSpeed; }

        Vector3 _pos = heroScroller.content.localPosition;
        if (_pos.x > contentWidth) { heroScroller.content.localPosition -= Vector3.right * contentWidth; }
        if (_pos.x < 0) { heroScroller.content.localPosition += Vector3.right * contentWidth; }

        int _sel = selected;
        float _h = Camera.main.orthographicSize * 2;
        float _w = _h * Camera.main.aspect;
        float _range = _w * .25f;
        float _center = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (Mathf.Abs(cards[i].transform.position.x - _center) < _range)
            {
                _sel = cards[i].index;
                break;
            }
        }

        if (selected != _sel)
        {
            UpdateSelectedCard(_sel);
        }
    }

    public void Setup(ShopHeroItem _pack)
    {
        heroUnlockPack = _pack;
        buyButton.shopItem = _pack;

        costCoins.text = $"{_pack.costCoins}";
        costDiamonds.text = $"{_pack.costDiamonds}";

        int _reps = heroUnlockPack.card.Length;
        contentWidth = _reps * cardWidth;
        _reps = _reps * Mathf.Max(Mathf.CeilToInt((float)minimumCards / _reps), 2);

        int j = 0;
        int m = heroUnlockPack.card.Length - 1;
        for (int i = 0; i < _reps; i++)
        {
            Instantiate(heroButtonPrefab, heroScroller.content).Setup(_pack.card[j], j);
            j++;
            if (j > m) { j = 0; }
        }

        cards = new ScrolledHeroButton[heroScroller.content.childCount];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = heroScroller.content.GetChild(i).GetComponent<ScrolledHeroButton>();
        }

        UpdateSelectedCard(0);
    }

    void UpdateSelectedCard(int _sel)
    {

        int l = heroUnlockPack.card.Length;
        selected = _sel;
        if (selected < 0) { selected += l; }
        if (selected >= l) { selected -= l; }

        selected = Mathf.Clamp(selected, 0, l - 1);

        HeroCard _card = heroUnlockPack.card[selected];

        cardElement.sprite = gemSprites[(int)_card.type];

        cardName.text = _card.Name;
        cardHealth.text = _card.maxHealth.ToString("F0");
        cardAttack.text = _card.attack.ToString("F0");
        cardDefense.text = _card.defense.ToString("F0");
        cardTier.text = _card.rarity.ToString();
    }

    // ---

    public void OpenHeroSummonScreen()
    {
        //   FirebaseManager.instance.gameManager.SetState(GameState.Title);
        FirebaseManager.instance.gameManager.OpenHeroSummonScreen();
    }

    public void Back()
    {
        FirebaseManager.instance.gameManager.ReturnToHome();
    }
}
