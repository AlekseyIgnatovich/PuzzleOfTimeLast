using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    public LoadingScreen loadingScreen;
    public LoginScreen loginScreen;

    public GameManager gameManager;
    public LoginRegisterUI loginRegisterUI;
    public bool enableAutoLogin;

    [Header("Managers")]
    public FirebaseAuthManager firebaseAuthManager;
    public FirestoreManager firestoreManager;

    private FirebaseUser firebaseUser;



    //Events//------------------------------------------------------------------------
    public delegate void OnUserLogin();
    public static OnUserLogin onUserLogin;

    public delegate void OnHomeScreenShowed();
    public static OnHomeScreenShowed onHomeScreenShow;

    ////------------------------------------------------------------------------------


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
        enableAutoLogin = PlayerPrefs.GetInt("Remember_User") == 1;
    }

    public void Start()
    {
        loadingScreen.gameObject.SetActive(true);
    }

    public void SetCurrentUser(FirebaseUser user)
    {
        Debug.Log("Current User: " + user.Email);
        firebaseUser = user;
        firestoreManager.LoadPlayerDataFromCloud(user.Email, user.UserId);
    }


    //This function is being called from the Start button at the Home Screen
    public void ActivateAuthenticator()
    {
        firebaseAuthManager.gameObject.SetActive(true);
        loginRegisterUI.gameObject.SetActive(true);
    }

    // ---

    public double GetHoursSinceLastLogin()
    {
        DateTime today = DateTime.Now;
        DateTime lastLogin = firestoreManager.playerData.lastLogin;
        TimeSpan difference = today - lastLogin;
        return difference.TotalHours;
    }

}
