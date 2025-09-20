using UnityEngine;
using TMPro;

public class BuyDiamondPopup : MonoBehaviour
{

    [SerializeField] HeroBase heroBase;
    [Space]
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textDescription;

    float cost;
    float diamonds;

    public void Setup(float _costUSD, float _diamonds)
    {
        gameObject.SetActive(true);

        cost = _costUSD;
        diamonds = _diamonds;

        textName.text = $"{diamonds} Diamonds";
        textDescription.text = $"Buy {diamonds} Diamonds for ${cost}?";
    }

    public void BuyDiamonds()
    {
        heroBase.ModifyDiamonds(diamonds);
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
