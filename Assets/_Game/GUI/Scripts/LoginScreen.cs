using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoginScreen : MonoBehaviour
{
    [Space(20)]
    [Header("UI Elements")]
    public TMP_InputField emaiLoginField;
    public TMP_InputField passwordLoginField;
    public Toggle rememberUser;

    public bool disableAutoLogin;
    int currentToggleValue = 0;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("Remember"))
        {
            currentToggleValue = 1;
            rememberUser.isOn = true;
        }

        if (rememberUser.isOn && PlayerPrefs.HasKey("Remember"))
        {
            Debug.Log("Rembember");
            if (disableAutoLogin)
            {
                rememberUser.isOn = false;
                currentToggleValue = 0;
                PlayerPrefs.DeleteKey("Remember");
            }

            if (FirebaseManager.instance == null)
            {
                Debug.LogError("Firebase manager is null, trying again");
                Invoke("OnEnable", 0.5f);
                return;
            }

            LeanTween.delayedCall(1f, () =>
            {
                FirebaseManager.instance.firebaseAuthManager.AutoLogin();
                gameObject.SetActive(false);
            });

        }
        else
        {
            gameObject.SetActive(true);
            Debug.Log("Dont remember");
            FirebaseManager.instance.loadingScreen.AddAmountToFill(50);
        }
    }

    public void LoginUser()
    {
        if (rememberUser.isOn)
        {
            PlayerPrefs.SetInt("Remember", 1);
        }
        else
        {
            PlayerPrefs.DeleteKey("Remember");
        }
        FirebaseManager.instance.firebaseAuthManager.Login(emaiLoginField.text, passwordLoginField.text);
    }

    public void UpdateRememberValue()
    {
        currentToggleValue = rememberUser.isOn ? 1 : 0;
    }
}
