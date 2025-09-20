using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopHeroReward : MonoBehaviour {

    [SerializeField] HeroBase heroBase;
    [SerializeField] Sprite[] elements;
    [Space]
    [SerializeField] Image imageCard;
    [SerializeField] Image imageHero;
    [SerializeField] Image imageElement;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] TextMeshProUGUI textAttack;
    [SerializeField] TextMeshProUGUI textDefense;
    [SerializeField] TextMeshProUGUI textDescription;
    [SerializeField] TextMeshProUGUI textTier;
    [SerializeField] TextMeshProUGUI textLevel;

    public void Setup(HeroCard _card) {
        if (_card == null) { return; }

        gameObject.SetActive(true);

        imageCard.sprite = heroBase.GetTierCard(_card.rarity);
        imageHero.sprite = _card.sprite;
        imageElement.sprite = elements[(int)_card.type];

        textName.text = $"{_card.Name}";
        textHealth.text = $"{_card.maxHealth}";
        textAttack.text = $"{_card.attack}";
        textDefense.text = $"{_card.defense}";
        textTier.text = $"{_card.rarity}";
        textLevel.text = $"0";

        textDescription.text = $"{_card.heroClass.description}";
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
