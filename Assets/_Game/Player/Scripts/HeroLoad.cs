using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroLoad : MonoBehaviour {

    [SerializeField] HeroBase heroBase;
    [Space]
    [SerializeField] Image frame;
    [SerializeField] Image heroImage;

    public void Setup(HeroItem _hero) {

        HeroCard _card = heroBase.heroCards[_hero.index];

        heroImage.sprite = _card.gameSprite;
        frame.sprite = heroBase.GetElementFrame(_card.type);
    }
}
