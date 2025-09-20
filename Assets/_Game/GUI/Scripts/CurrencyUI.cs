using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour {

    [SerializeField] HeroBase heroBase;
    [Space]
    [SerializeField] Currency currency;
    [Space]
    [SerializeField] TextMeshProUGUI textCurrency;

    private void OnEnable() {
        heroBase.OnCoinsChange += UpdateCurrency;
        heroBase.OnDiamondsChange += UpdateCurrency;
        UpdateCurrency();
    }
    private void OnDisable() {
        heroBase.OnCoinsChange -= UpdateCurrency;
        heroBase.OnDiamondsChange -= UpdateCurrency;
    }

    private void Start() {
        UpdateCurrency();
    }


    void UpdateCurrency() {
        switch (currency) {
            case Currency.Coins:
                textCurrency.text = heroBase.coins.ToString("F0"); break;
            case Currency.Diamonds:
                textCurrency.text = heroBase.diamonds.ToString("F0"); break;
        }
    }
}
