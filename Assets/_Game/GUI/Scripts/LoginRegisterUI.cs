using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
#if UNITY_EDITOR
using UnityEditor.Tilemaps;
using UnityEditor;
#endif
public class LoginRegisterUI : MonoBehaviour
{
    [SerializeField] GameObject loginScreen;
    [SerializeField] GameObject registerScreen;
    [SerializeField] AuthInitType initType;
    [SerializeField] TextMeshProUGUI popUpTMP;
    [SerializeField] GameObject popUpWindow;

    void OnEnable()
    {
        FirebaseManager.onUserLogin += ShowLoginMessage;
        Debug.Log("Login Screen enable");
        if (FirebaseManager.instance.firebaseAuthManager.user != null)
        {
            Debug.Log("user is already logged in: " + FirebaseManager.instance.firebaseAuthManager.user.Email);
            this.ShowLoginMessage();
        }
    }
    void OnDisable()
    {
        FirebaseManager.onUserLogin -= ShowLoginMessage;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        if (initType == AuthInitType.Login)
        { StartLoginScreen(); }
        else
        {
            StartRegisterScreen();
        }
    }

    public void StartLoginScreen()
    {
        loginScreen.SetActive(true);
        registerScreen.SetActive(false);
        if (FirebaseManager.instance.firebaseAuthManager.user != null)
        {
            Debug.Log("user is already logged in: " + FirebaseManager.instance.firebaseAuthManager.user.Email);
            this.ShowLoginMessage();
        }
    }

    public void StartRegisterScreen()
    {
        registerScreen.SetActive(true);
        loginScreen.SetActive(false);
    }

    #region PopUp Window
    void ShowPopUpMessageWindow(float duration = -1)
    {
        StartCoroutine(ShowWindowCoroutine(duration));
    }

    IEnumerator ShowWindowCoroutine(float duration = -1)
    {
        yield return new WaitForSeconds(0.2f);

        popUpWindow.SetActive(true);
        if (duration == -1)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }
    }

    void ShowLoginMessage()
    {
        popUpTMP.text = "Login succesful";
        ShowPopUpMessageWindow(1);
    }

    public void ShowErrorMessage(string _text)
    {
        popUpTMP.text = _text;
        ShowPopUpMessageWindow();
    }

    //This function is being called from the popup Window closeBtn 
    public void ClosePopUp()
    {
        if (FirebaseManager.instance.firebaseAuthManager.user != null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            popUpWindow.gameObject.SetActive(false);
        }
    }
    #endregion

}

// This types are to set the initial state of the Login/Register screen
public enum AuthInitType { Login, Register }
