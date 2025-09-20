using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitanManequin : MonoBehaviour {

    [SerializeField] TitanData titanData;
    [Space]
    [SerializeField] Transform manequin;
    [SerializeField] SpriteRenderer picture;
    [SerializeField] Animator animator;
    [SerializeField] Healthbar healthbar;
    [SerializeField] TextMeshProUGUI timerText;

    MobCard mobCard;

    private void Start() {
        Setup();
        UpdateTimer();
    }

    private void Update() {
        UpdateTimer();
    }

    public void Setup() {
        mobCard = titanData.GetTitan();
        animator.runtimeAnimatorController = mobCard.animator;
        healthbar.UpdateHealth(titanData.GetTitanHealth(), titanData.GetTitanMaxHealth(), false);

        animator.Play(0);
        picture.size = mobCard.size * 1.5f;
        manequin.localScale = Vector3.one;
    }

    void UpdateTimer() {
        float _tm = Mathf.Max(titanData.GetTitanTimer());
        float _min = Mathf.FloorToInt(_tm / 60);
        float _sec = Mathf.FloorToInt(_tm - (_min * 60));
        timerText.text = $"{_min} : {_sec}";
    }
}
