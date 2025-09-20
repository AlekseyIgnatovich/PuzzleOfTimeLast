using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityInfoPanel : MonoBehaviour {

    [SerializeField] Image picture;
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] TextMeshProUGUI abilityDescription;

    public void Setup(AbilityButton _button) {
        gameObject.SetActive(true);

        HeroSpecial _ability = _button.GetSpecial();
        picture.sprite = _ability.sprite;
        abilityName.text = _ability.Name;
        abilityDescription.text = _ability.description;
    }
}
