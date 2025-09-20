using UnityEngine;
using UnityEngine.UI;

public class ShopPanelButton : MonoBehaviour
{

    [SerializeField] Image image;
    [SerializeField] GameObject panel;
    [Space]
    [SerializeField] Sprite[] buttonSprites;

    public void UpdateButton()
    {
        image.sprite = buttonSprites[panel.activeSelf ? 1 : 0];
    }

}
