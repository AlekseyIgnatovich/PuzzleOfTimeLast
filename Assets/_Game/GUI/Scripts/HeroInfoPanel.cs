using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroInfoPanel : MonoBehaviour {

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

    public void Setup(Hero _hero) {
        gameObject.SetActive(true);

        frame.sprite = frames[(int)_hero.rarity];
        picture.sprite = _hero.card.sprite;
        textName.text = _hero.card.Name;
        textTier.text = _hero.rarity.ToString();
        textHealth.text = $"{_hero.health.ToString("F0")} /\n{_hero.maxHealth.ToString("F0")}";
        textAttack.text = $"{_hero.attack.ToString("F0")}";
        textDefense.text = $"{_hero.defense.ToString("F0")}";
        textLevel.text = $"{_hero.level}";

        textClassDescription.text = _hero.card.heroClass.description;

        element.sprite = elements[(int)_hero.type];
    }
}
