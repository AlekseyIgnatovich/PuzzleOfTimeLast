using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GoldenPassItemController : MonoBehaviour
{
    [Header("Properties")]
    public RewardPackage reward;
    public float diamondsPrice;

    [Header("Debug Info")]
    public int level;
    public bool unlocked, claimed;

    [Header("Components")]
    public Button btn;
    public GameObject check;
    public TMPro.TMP_Text itemNameTxt, diamondsPriceTxt;
    public Image itemImg, panelImg;
    public Sprite lockedSpr, readySpr, claimedSpr;

    public delegate void OnClaim(GoldenPassItemController _item);
    public OnClaim onClaim;

    // ---

    private void OnEnable()
    {
        btn.onClick.AddListener(OnClick);
    }
    private void OnDisable()
    {
        btn.onClick.RemoveListener(OnClick);
    }

    // ---

    public void SetState(bool _unlocked, bool _claimed)
    {
        unlocked = _unlocked;
        claimed = _claimed;
        btn.interactable = unlocked && !claimed;
        check.SetActive(claimed);
        diamondsPriceTxt.transform.parent.gameObject.SetActive(btn.interactable && diamondsPrice > 0);
        diamondsPriceTxt.text = diamondsPrice.ToString();
        diamondsPriceTxt.color = CanBuy() ? Color.white : new Color(1f, 0.5f, 0.5f, 1f);
        panelImg.sprite = !unlocked ? lockedSpr : (claimed ? claimedSpr : readySpr);
    }

    bool CanBuy()
    {
        return FirebaseManager.instance.gameManager.rewardManager.heroBase.diamonds >= diamondsPrice;
    }

    // ---

    void OnClick()
    {
        if (!unlocked) return;
        if (claimed) return;
        if (!CanBuy()) return;
        onClaim?.Invoke(this);
    }
}