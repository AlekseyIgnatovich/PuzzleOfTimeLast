using UnityEngine;

public class ItemShowButton : MonoBehaviour
{

    [SerializeField] ItemsData data;
    [SerializeField] ItemCase itemCase;
    [Space]
    [SerializeField] GameObject buttonOn;
    [SerializeField] GameObject buttonOff;
    [SerializeField] bool startsOn;

    private void OnEnable()
    {
        data.itemsShow = ItemCase.Game;
        data.OnShowItems += UpdateButtons;
        UpdateButtons();
    }
    private void OnDisable()
    {
        data.OnShowItems -= UpdateButtons;
    }

    private void Start()
    {
        UpdateButtons();
    }

    void UpdateButtons()
    {
        if (itemCase == data.itemsShow)
        {
            buttonOn.SetActive(true);
            buttonOff.SetActive(false);
        }
        else
        {
            buttonOn.SetActive(false);
            buttonOff.SetActive(true);
        }
    }
}
