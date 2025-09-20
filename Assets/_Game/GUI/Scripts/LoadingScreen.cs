using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] float loadingSpeed;

    public bool isLoading;
    float currentFillAmount;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartLoading();
        AddAmountToFill(50);
    }

    private void Update()
    {
        if (!isLoading) return;

        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, currentFillAmount, loadingSpeed * Time.deltaTime);
        if (fillImage.fillAmount >= 1)
        {
            isLoading = false;

            gameObject.SetActive(false);
            if (GameManager.instance.isFirstTime)
            {
                GameManager.instance.SetState(GameState.MapSelect);
            }
        }
    }

    void StartLoading()
    {
        currentFillAmount = 0;
        isLoading = true;
    }

    public void AddAmountToFill(float _amount)
    {
        if (currentFillAmount + (_amount / 100) >= 1)
        {
            currentFillAmount = 1;
        }
        currentFillAmount += (_amount / 100);
    }
}
