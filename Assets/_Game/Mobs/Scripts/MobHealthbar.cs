using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobHealthbar : MonoBehaviour
{

    [SerializeField] GameObject counterObject;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] TextMeshProUGUI textCounter;

    Vector2 barsize;

    private void Awake()
    {
        barsize = healthBar.rectTransform.sizeDelta;
    }

    public void UpdateCounter(int _count)
    {
        counterObject.SetActive(_count > 0);
        textCounter.text = $"{_count}";
    }

    public void UpdateHealth(float min, float max)
    {
        healthBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(min / max, 0, 1) * barsize.x, barsize.y);
        textHealth.text = $"{FormatNumber(min)}/{FormatNumber(max)}";
    }

    private string FormatNumber(float value)
    {
        if (value >= 10000)
        {
            float abbreviatedValue = value / 1000f;
            if (abbreviatedValue % 1 == 0)
            {
                return $"{abbreviatedValue:F0}k";
            }
            else
            {
                return $"{abbreviatedValue:F1}k";
            }
        }
        return value.ToString("F0");
    }
}