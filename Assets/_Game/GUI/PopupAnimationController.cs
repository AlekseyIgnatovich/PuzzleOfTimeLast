using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAnimationController : MonoBehaviour
{
    [SerializeField] GameObject popupWindow;
    [SerializeField] GameObject button;
    [SerializeField] CanvasGroup alphaBG;
    [SerializeField] WindowAnimationType type;
    [SerializeField] float animTime;
    [SerializeField] float alphaTime = 0f;
    [SerializeField] bool bounceButton;
    [SerializeField] Vector2 initLocalPos;

    bool isShowing;
    bool hasPurchased;
    Coroutine buttonCoroutine;
    public void Initialize(bool _purchased)
    {
        //LeanTween.cancel(popupWindow);
        //LeanTween.cancel(alphaBG.gameObject);
        if (type == WindowAnimationType.ScaleAll)
        {
            popupWindow.transform.localScale = Vector2.zero;
        }
        else
        {
            popupWindow.transform.localPosition = initLocalPos;
        }

        if (alphaBG != null)
        {
            alphaBG.alpha = 0;
        }
        if (button != null)
        {
            button.transform.localScale = Vector2.zero;
        }

        hasPurchased = _purchased;
    }

    public void ShowPopupWindow()
    {
        if (hasPurchased) return;
        if (alphaBG != null)
        {
            LeanTween.value(alphaBG.gameObject, 0f, 1f, alphaTime)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnUpdate((float alpha) =>
                 {
                     alphaBG.alpha = alpha;
                 });
        }

        switch (type)
        {
            case WindowAnimationType.ScaleAll:
                Vector2 finalScale = Vector2.one;
                StartScaleAnimation(finalScale);
                break;
            case WindowAnimationType.FallDown:
                Vector2 finalPosition = new Vector2(popupWindow.transform.localPosition.x, 0);
                StartMoveAnimation(finalPosition);
                break;
            case WindowAnimationType.SideLeft:
                break;
            case WindowAnimationType.SideRight:
                break;
            case WindowAnimationType.JumpUp:
                Vector2 finalPosition2 = new Vector2(popupWindow.transform.localPosition.x, 0);
                StartMoveAnimation(finalPosition2);
                break;
            default:
                break;
        }
    }
    void StartMoveAnimation(Vector2 _finalPos)
    {
        LeanTween.moveLocal(popupWindow, _finalPos, animTime).setDelay(alphaTime).setEaseOutBack().setOnComplete(StartButtonAnimation);
    }

    void StartScaleAnimation(Vector2 _finalScale)
    {
        LeanTween.scale(popupWindow, _finalScale, animTime).setDelay(alphaTime).setEaseOutBack().setOnComplete(StartButtonAnimation);
    }

    void StartButtonAnimation()
    {
        if (button == null) return;
        LeanTween.scale(button, Vector2.one, 0.27f).setEaseOutBounce().setOnComplete(() =>
        {
            isShowing = true;
            //buttonCoroutine = StartCoroutine(ScalingCoroutine());
        });
    }

    IEnumerator ScalingCoroutine()
    {
        yield return new WaitForSeconds(0.4f);

        yield return new WaitForSeconds(1f);
        if (isShowing)
        {
            buttonCoroutine = StartCoroutine(ScalingCoroutine());
        }
    }

    public void HidePopupWindow()
    {
        isShowing = false;
        if (buttonCoroutine != null)
        {
            StopCoroutine(buttonCoroutine);
        }
        gameObject.SetActive(false);
        LeanTween.cancel(button);
        LeanTween.cancel(popupWindow);
    }
}

public enum WindowAnimationType { ScaleAll, FallDown, SideLeft, SideRight, JumpUp }
