using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {

    [SerializeField] Image image;

    Hero hero;

    public void Setup(Hero _hero) {
        hero = _hero;

        image.sprite = hero.card.heroClass.special.sprite;
    }

    public HeroSpecial GetSpecial() {
        return hero.card.heroClass.special;
    }
}
