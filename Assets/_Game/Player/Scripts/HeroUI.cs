using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroUI : MonoBehaviour
{

    [SerializeField] Sprite[] frames;
    [SerializeField] Sprite[] labels;
    [SerializeField] GameObject[] glows;
    [Space]
    public Hero hero;
    public int number;
    [SerializeField] Transform body;
    [SerializeField] Image frame;
    [SerializeField] Image label;
    [SerializeField] Image picture;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textAttack;
    [SerializeField] TextMeshProUGUI textDefense;
    [Space]
    public GameObject targetMark;

    public void SetUI(HeroCard _card)
    {
        textName.text = _card.Name;
        picture.sprite = _card.gameSprite;
        frame.sprite = frames[(int)_card.type];
        label.sprite = labels[(int)_card.type];
        UpdateGLow();

        number = transform.GetSiblingIndex();
    }
    public void UpdateAttack(float value)
    {
        textAttack.text = $"{value}";
    }
    public void UpdateDefense(float value)
    {
        textDefense.text = $"{value}";
    }

    public void Refresh(Transform _parent)
    {
        transform.SetParent(_parent);
        transform.SetSiblingIndex(number);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    void UpdateGLow()
    {
        int s = (int)hero.type;
        for (int i = 0; i < glows.Length; i++)
        {
            glows[i].SetActive(i == s);
        }
    }
}
