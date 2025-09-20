using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFailedScreenUI : MonoBehaviour
{
    [SerializeField] GameplayManager gameplayManager;

    [Header("UI ELEMENTS")]
    [SerializeField] CanvasGroup bgCanvasGroup;
    [SerializeField] GameObject titleFrame;
    [SerializeField] GameObject title;
    [SerializeField] GameObject text;
    [SerializeField] GameObject continueTxt;
    [SerializeField] GameObject[] buttons;

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        //Initialize();
        StartAnimation();
    }

    void Initialize()
    {
        bgCanvasGroup.alpha = 0;
        titleFrame.transform.localScale = new Vector3(1, 0);
        title.transform.localScale = Vector2.zero;
        text.transform.localScale = Vector2.zero;
        foreach (var item in buttons)
        {
            item.transform.localScale = Vector2.zero;
        }
    }


    void StartAnimation()
    {
        LeanTween.value(gameObject, 0, 0.8f, 0.5f).setOnUpdate((float val) =>
        {
            bgCanvasGroup.alpha = val;
        }).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            LeanTween.scaleY(titleFrame, 1f, 0.16f).setEaseOutSine();
            LeanTween.scale(title, Vector2.one, 0.23f).setEaseOutBack().setDelay(0.15f);
            LeanTween.scaleY(text, 1f, 0.24f).setEaseInOutSine().setDelay(0.24f).setOnComplete(() =>
            {
                LeanTween.scale(continueTxt, Vector2.one, 0.18f);
                LeanTween.scale(buttons[0], Vector2.one, 0.17f).setDelay(0.2f).setEaseOutBack();
                LeanTween.scale(buttons[1], Vector2.one, 0.17f).setDelay(0.5f).setEaseOutBack();
                LeanTween.scale(buttons[2], Vector2.one, 0.17f).setDelay(1f).setEaseOutBack();
                LeanTween.scale(buttons[3], Vector2.one, 0.17f).setDelay(1f).setEaseOutBack();
            });
        });
    }
}
