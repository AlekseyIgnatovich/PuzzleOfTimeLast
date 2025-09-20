using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using TMPro;
using Microsoft.SqlServer.Server;

public class FirebaseAuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencystatus;
    public FirebaseAuth auth = null;
    public FirebaseUser user = null;

    int loginTries = 0;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencystatus = task.Result;
            if (dependencystatus == DependencyStatus.Available)
            {
                InitializeFirebaseAuth();
            }
            else
            {
                Debug.LogError("Coudnt resolve all firebase dependencies: " + dependencystatus);
            }
        });
    }
    private void OnDisable()
    {
        if (FirebaseManager.instance.enableAutoLogin) return;
        //auth.StateChanged -= AuthStateChanged;
    }

    private void InitializeFirebaseAuth()
    {
        auth = FirebaseAuth.DefaultInstance;
        if (!FirebaseManager.instance.enableAutoLogin) return;
        //auth.SignOut();
        //AutoLogin();
    }

    public void AutoLogin()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            if (auth == null)
            {
                Debug.Log("Auth null");
            }
            if (auth.CurrentUser == null)
            {
                Debug.Log("current user is null");
            }
            loginTries++;
            Debug.Log("tries" + loginTries);
            if (loginTries >= 2)
            {
                FirebaseManager.instance.loadingScreen.AddAmountToFill(50);
                FirebaseManager.instance.loginRegisterUI.StartRegisterScreen();
            }
            else
            {
                LeanTween.delayedCall(1f, () =>
                {
                    AutoLogin();
                });
            }
        }
        else
        {
            AuthStateChanged(this, null);
        }
        //auth.StateChanged += AuthStateChanged;
        //auth.SignOut();
    }

    //Track state changes of the auth object
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        user = auth.CurrentUser;
        FirebaseManager.instance.SetCurrentUser(user);
        FirebaseManager.instance.loginRegisterUI.StartLoginScreen();
        // if (auth.CurrentUser != user)
        // {
        //     bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
        //     if (!signedIn && user != null)
        //     {
        //         Debug.Log("Signed out " + user.UserId);
        //         FirebaseManager.instance.loginRegisterUI.StartLoginScreen();
        //     }
        //     if (signedIn)
        //     {
        //         Debug.Log("Signed in " + user.UserId);
        //     }
        // }
    }

    public void Login(string email, string password)
    {
        StartCoroutine(LoginAsync(email, password));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "login Failed because: ";

            switch (authError)
            {
                case AuthError.EmailAlreadyInUse:
                    failedMessage += "Email is already in use";
                    break;
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage += "Login Failed";
                    break;
            }
            Debug.Log(failedMessage);
            FirebaseManager.instance.loginRegisterUI.ShowErrorMessage(failedMessage);
        }
        else
        {
            user = loginTask.Result.User;
            Debug.LogFormat("{0} You have logged in succesfully ", user.DisplayName);
            FirebaseManager.instance.SetCurrentUser(user);
        }
    }


    public void Register(string name, string email, string password, string confirmPass)
    {
        Debug.Log("The user name is: " + name);
        StartCoroutine(RegisterAsync(name, email, password, confirmPass));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPass)
    {
        string popUpMessage;
        if (name == "")
        {
            popUpMessage = "Name is empty";
        }
        else if (email == "")
        {
            popUpMessage = "Email is empty";
        }
        else if (password != confirmPass)
        {
            popUpMessage = "Passwords don't match";
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                popUpMessage = "Registration failed ";
                switch (authError)
                {
                    case AuthError.EmailAlreadyInUse:
                        popUpMessage += "Email is already in use";
                        break;
                    case AuthError.InvalidEmail:
                        popUpMessage += "Invalid email adress";
                        break;
                    case AuthError.WrongPassword:
                        popUpMessage += "Invalid Password";
                        break;
                    case AuthError.MissingEmail:
                        popUpMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        popUpMessage += "Password is missing";
                        break;
                    default:
                        popUpMessage += "Something went wrong";
                        break;
                }

                //Debug.LogError(failedMessage);
                FirebaseManager.instance.loginRegisterUI.ShowErrorMessage(popUpMessage);
            }
            else
            {
                //Gets the user after a registration successfull
                user = registerTask.Result.User;
                FirebaseManager.instance.firestoreManager.CreateUserData(user.Email, user.UserId, name);
                FirebaseManager.instance.SetCurrentUser(user);
                UserProfile userProfile = new UserProfile();
                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    //Delete the user if the user update failed
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = "Profile update failed: ";

                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Invalid email adress";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Invalid Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage += "Something went wrong";
                            break;
                    }

                    Debug.Log(failedMessage);
                }
                else
                {
                    Debug.Log("Registration succesfull: " + user.DisplayName);
                }
            }
        }
    }
}
