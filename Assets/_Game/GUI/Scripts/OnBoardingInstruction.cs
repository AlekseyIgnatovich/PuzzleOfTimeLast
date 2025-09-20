using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnBoardingInstruction : MonoBehaviour
{
    [SerializeField] InstructionInfo instructionInfo;
    [SerializeField] CanvasGroup onBoardingBG;
    [SerializeField] RectTransform handImg;
    [SerializeField] RectTransform popUpMessage;
    [SerializeField] RectTransform tipMessage;
    [SerializeField] TextMeshProUGUI messageTMP;
    [SerializeField] TextMeshProUGUI tipTMP;
    [SerializeField] GameObject okBtn;
    [SerializeField] float initDelay;
    [SerializeField] bool hideHandOnStart;
    [SerializeField] bool hideInstructionOnTouch;
    [SerializeField] bool moveHandToTarget;
    [SerializeField] bool hasOkButton;


    [HideInInspector] public int selectedMap;
    [HideInInspector] public UIMap currentMap;

    public GameObject target;
    bool isShowingTutorial, isShowingHand;


    private void OnEnable()
    {
        Initialize();
    }
    private void Initialize()
    {
        handImg.localScale = Vector2.zero;
        popUpMessage.localScale = Vector2.zero;
        tipMessage.localScale = Vector2.zero;
        if (hasOkButton)
        {
            okBtn.transform.localScale = Vector2.zero;
        }
        if (target == null)
        {
            target = transform.parent.gameObject;
        }
        SetTutorialConfig();
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0) || !isShowingTutorial) return;

        if (isShowingHand && instructionInfo.hideHandOnTouch)
        {
            handImg.gameObject.SetActive(false);
        }
        else if (hideInstructionOnTouch)
        {
            Debug.Log("Hiding instruction");
            onBoardingBG.transform.parent = transform;
            gameObject.SetActive(false);
        }
    }




    private void SetTutorialConfig()
    {
        Debug.Log("Mostrando Mapas");
        onBoardingBG.gameObject.SetActive(true);
        onBoardingBG.alpha = 0f;
        onBoardingBG.transform.parent = target.transform;
        onBoardingBG.transform.SetSiblingIndex(0);
        LeanTween.value(gameObject, 0, .7f, .7f).setOnUpdate((newAlpha) =>
        {
            onBoardingBG.alpha = newAlpha;
        }).setDelay(initDelay).setOnComplete(() =>
        {
            messageTMP.text = instructionInfo.message;
            tipTMP.text = instructionInfo.showTip ? instructionInfo.tipMessage : "";
            popUpMessage.localScale = new Vector2(1, 0);
            tipMessage.localScale = new Vector2(0, 0);
            LeanTween.scale(popUpMessage, Vector2.one, 0.34f).setEaseOutBack().setDelay(0.25f).setOnComplete(() =>
            {
                isShowingTutorial = true;
                if (instructionInfo.showTip)
                {
                    LeanTween.scale(tipMessage, Vector2.one, 0.34f).setEaseOutBack().setDelay(0.25f).setOnComplete(() =>
                    {
                        if (hasOkButton)
                        {
                            LeanTween.scale(okBtn, Vector2.one, 0.34f).setEaseOutBounce().setDelay(0.15f);
                        }

                    });
                }
                else if (hasOkButton)
                {
                    LeanTween.scale(okBtn, Vector2.one, 0.34f).setEaseOutBounce().setDelay(0.15f);
                }
            });
            if (hideHandOnStart) return;
            if (moveHandToTarget)
            {
                Vector2 newPos = target.transform.position;
                newPos.x -= 0.25f;
                newPos.y += 1.3f;
                handImg.transform.position = newPos;
            }
            LeanTween.scale(handImg, Vector2.one, 0.26f).setEaseOutBack().setOnComplete(() =>
            {
                isShowingHand = true;
                StartHandAnimation();
            });
        });
    }

    void StartHandAnimation()
    {
        switch (instructionInfo.handAnimationType)
        {
            case HandAnimationType.Pointing:
                LeanTween.moveY(handImg.gameObject, handImg.position.y - 0.3f, .68f).setLoopPingPong().setEaseInOutSine().setDelay(0.3f);
                break;
            case HandAnimationType.Touching:
                LeanTween.scale(handImg.gameObject, handImg.localScale * 0.9f, 0.888f).setEasePunch().setDelay(1.5f).setLoopPingPong();
                break;
            default:
                break;
        }
    }

}

[System.Serializable]
public class InstructionInfo
{
    public string message;
    public string tipMessage;
    public HandAnimationType handAnimationType;
    public bool hideHandOnTouch;
    public bool showTip;
}

public enum HandAnimationType { Pointing, Touching }
