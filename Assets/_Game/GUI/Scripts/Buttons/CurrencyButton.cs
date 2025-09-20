using UnityEngine;
using TMPro;

public class CurrencyButton : MonoBehaviour {

    [SerializeField] HeroBase heroBase;
    [SerializeField] ItemsData data;
    [SerializeField] Currency currency;
    [Space]
    [SerializeField] TextMeshProUGUI textCurrency;
    [SerializeField] TextMeshProUGUI textCurrency1;
    [SerializeField] GameObject buttonOn;
    [SerializeField] GameObject buttonOff;


    private void OnEnable() {
        data.OnSwitchCurrency += UpdateButtons;
        heroBase.OnCoinsChange += UpdateCurrency;
        heroBase.OnDiamondsChange += UpdateCurrency;
        UpdateButtons();
        UpdateCurrency();
    }
    private void OnDisable() {
        data.OnSwitchCurrency -= UpdateButtons;
        heroBase.OnCoinsChange -= UpdateCurrency;
        heroBase.OnDiamondsChange -= UpdateCurrency;
    }

    private void Start() {
        UpdateButtons();
        UpdateCurrency();
    }

    void UpdateButtons() {
        if (currency == data.selectedCurrency) {
            buttonOn.SetActive(true);
            buttonOff.SetActive(false);
        } else {
            buttonOn.SetActive(false);
            buttonOff.SetActive(true);
        }
    }

    void UpdateCurrency() {
        switch (currency) {
            case Currency.Coins:
                textCurrency.text = heroBase.coins.ToString("F0"); break;
            case Currency.Diamonds:
                textCurrency.text = heroBase.diamonds.ToString("F0"); break;
        }
        textCurrency1.text = textCurrency.text;
    }
}
