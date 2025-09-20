using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBtnController : MonoBehaviour
{
    [SerializeField] RectTransform notification;
    [SerializeField] Button btn;
    [SerializeField] notificationType type;
    public bool hasNotification;


    // private void OnEnable()
    // {
    //     FirebaseManager.onHomeScreenShow += Initialize;
    // }

    // private void OnDisable()
    // {
    //     FirebaseManager.onHomeScreenShow -= Initialize;
    // }
    public void Init(bool _active)
    {
        if (_active)
        {
            Initialize();
        }
        else
        {
            notification.gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        //Debug.Log("Inicializando boton");
        CheckNotification();
        if (notification == null) return;
        //notification.gameObject.SetActive(false);
        //LeanTween.scale(notification, Vector2.zero, 0f);
        AnimateIn();
    }
    void CheckNotification()
    {
        switch (type)
        {
            case notificationType.Titan:
                // if (FirebaseManager.instance.gameManager.goldenPassBtnPressed) break;
                // hasNotification = FirebaseManager.instance.firestoreManager.titanOn;
                break;
            case notificationType.Reward:
                hasNotification = FirebaseManager.instance.firestoreManager.rewardOn;
                break;
            case notificationType.GoldenPass:
                if (FirebaseManager.instance.gameManager.goldenPassBtnPressed) break;
                hasNotification = FirebaseManager.instance.firestoreManager.goldenPassOn;
                break;
            case notificationType.DailyDeal:
                hasNotification = FirebaseManager.instance.firestoreManager.dailyDealOn;
                break;
            case notificationType.DailySpin:
                hasNotification = FirebaseManager.instance.firestoreManager.dailySpinOn;
                break;
            case notificationType.None:
                hasNotification = false;
                break;
            default:
                hasNotification = false;
                break;
        }
    }

    void AnimateIn()
    {
        notification.gameObject.SetActive(true);
    }

    public enum notificationType { Titan, Reward, DailyDeal, DailySpin, GoldenPass, None }

    public void OnBtnPressed()
    {
        Debug.Log("Button pressed");
        if (hasNotification && notification != null)
        {
            notification.gameObject.SetActive(false);
            //LeanTween.scale(notification, Vector2.zero, 0f);

        }
        DisableNotification();
    }

    void DisableNotification()
    {
        hasNotification = false;
        switch (type)
        {
            case notificationType.Titan:
                FirebaseManager.instance.gameManager.titanBtnPressed = true;
                break;
            case notificationType.Reward:
                FirebaseManager.instance.gameManager.rwrdBtnPressed = true;
                break;
            case notificationType.GoldenPass:
                FirebaseManager.instance.gameManager.goldenPassBtnPressed = true;
                break;
            case notificationType.DailyDeal:
                FirebaseManager.instance.gameManager.dailyDealBtnPressed = true;
                break;
            case notificationType.DailySpin:
                FirebaseManager.instance.firestoreManager.dailySpinOn = hasNotification;
                break;
            default:
                hasNotification = false;
                break;
        }
    }
}
