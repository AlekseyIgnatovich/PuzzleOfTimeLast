using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobInfoPanel : MonoBehaviour {

    [Space]
    [SerializeField] Image picture;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textLevel;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] TextMeshProUGUI textAttack;
    [SerializeField] TextMeshProUGUI textDefense;

    public void Setup(Mob _mob) {
        gameObject.SetActive(true);

        picture.sprite = _mob.card.sprite;
        textName.text = _mob.card.Name;
        textHealth.text = $"{_mob.health.ToString("F0")} /\n{_mob.maxHealth.ToString("F0")}";
        textAttack.text = $"{_mob.attack.ToString("F0")}";
        textDefense.text = $"{_mob.defense.ToString("F0")}";
        textLevel.text = $"{_mob.level}";
    }
}
