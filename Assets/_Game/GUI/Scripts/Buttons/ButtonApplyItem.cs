using UnityEngine;

public class ButtonApplyItem : UIButton {

    [SerializeField] InfoPopup infoPopup;
    [SerializeField] GemPopup gemPopup;

    public override void Execute() {
        if (infoPopup != null) { infoPopup.ApplyItem(); }
        if (gemPopup != null) { gemPopup.ApplyItem(); }
    }

}
