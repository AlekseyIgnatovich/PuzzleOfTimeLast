using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyPanelUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI headerTMP;
    [SerializeField] GameObject backBtn;

    public void OnHomeButtonPressed()
    {
        gameManager.ReturnToHome();
    }

    public void OnDiamondPressed()
    {
        gameManager.OpenDiamondsIAPScreen();
    }

    public void ToggleShowHeaderText(bool _show)
    {
        headerTMP.gameObject.SetActive(_show);
    }

    public void SetText(string _text)
    {
        headerTMP.text = _text;
    }

    public void ToggleShowPanel(bool _show)
    {
        if (gameManager.isFirstTime)
        {
            _show = false;
        }
        gameObject.SetActive(_show);
    }

    public void ToggleBackButton(bool _show)
    {
        backBtn.gameObject.SetActive(_show);
    }
}
