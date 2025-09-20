using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LotteryRewardPanel : MonoBehaviour {

    [SerializeField] HeroBase heroBase;
    [Space]
    [SerializeField] Image cardPicture;
    [SerializeField] TextMeshProUGUI textCardHealth;
    [SerializeField] TextMeshProUGUI textCardAttack;
    [SerializeField] TextMeshProUGUI textCardDefense;

    LotteryBox box;

    public void CollectRewards() {
        heroBase.AddCardToInventory(box.GetCard());
        gameObject.SetActive(false);
    }

    public void Open(LotteryBox _box) {
        if (_box == null) { return; }

        gameObject.SetActive(true);

        box = _box;
        box.RandomiseChoise();
        HeroCard _card = box.GetCard();

        cardPicture.sprite = _card.cardSprite;
        textCardHealth.text = $"{_card.maxHealth.ToString("F0")}";
        textCardAttack.text = $"{_card.attack.ToString("F0")}";
        textCardDefense.text = $"{_card.defense.ToString("F0")}";
    }
}
