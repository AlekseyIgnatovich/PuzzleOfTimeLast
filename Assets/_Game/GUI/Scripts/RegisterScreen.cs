using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterScreen : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPassRegisterField;

    public void RegisterUser()
    {
        FirebaseManager.instance.firebaseAuthManager.Register(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPassRegisterField.text);
    }
}
