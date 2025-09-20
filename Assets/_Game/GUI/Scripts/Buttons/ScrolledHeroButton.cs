using UnityEngine;
using UnityEngine.UI;

public class ScrolledHeroButton : MonoBehaviour {

    [SerializeField] GameObject[] glows;
    [SerializeField] Image image;
    public int index;

    HeroCard card;

    public void Setup(HeroCard _card, int _index) {
        card = _card;
        index = _index;
        image.sprite = _card.sprite;
        glows[(int)_card.type].SetActive(true);
    }

}
