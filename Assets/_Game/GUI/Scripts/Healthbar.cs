using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{

    [Tooltip("Set bar if you wish to disable the healthbar when health reaches 0")]
    [SerializeField] GameObject bar;
    [SerializeField] Image healthBar;
    public TextMeshProUGUI textHealth;

    Vector2 barsize;

    private void Awake()
    {
        barsize = healthBar.rectTransform.sizeDelta;
    }

    public void UpdateHealth(float min, float max, bool drawmax)
    {
        if (bar != null) { bar.SetActive(min > 0); }
        healthBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(min / max, 0, 1) * barsize.x, barsize.y);
        if (textHealth != null) { textHealth.text = drawmax ? $"{min.ToString("F0")}/{max.ToString("F0")}" : $"{min.ToString("F0")}"; }
    }
}
