using UnityEngine;

public class ShopShowButton : MonoBehaviour {

    [SerializeField] ItemsData data;
    [SerializeField] int shopShow;
    [Space]
    [SerializeField] GameObject buttonOn;
    [SerializeField] GameObject buttonOff;

    private void OnEnable() {
        data.OnShowShop += UpdateButtons;
    }
    private void OnDisable() {
        data.OnShowShop -= UpdateButtons;
    }

    private void Start() {
        UpdateButtons();
    }

    void UpdateButtons() {
        if (shopShow == data.shopShow) {
            buttonOn.SetActive(true);
            buttonOff.SetActive(false);
        } else {
            buttonOn.SetActive(false);
            buttonOff.SetActive(true);
        }
    }

}
