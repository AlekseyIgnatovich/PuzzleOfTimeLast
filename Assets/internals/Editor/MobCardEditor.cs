using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MobCard))]
public class MobCardEditor : Editor {

    MobCard card;

    private void OnEnable() {
        //target is by default available for you
        //because we inherite Editor
        card = target as MobCard;
    }

    //Here is the meat of the script
    public override void OnInspectorGUI() {
        //Draw whatever we already have in SO definition
        base.OnInspectorGUI();
        //Guard clause
        if (card.sprite == null) { return; }

        Texture2D textureBust = AssetPreview.GetAssetPreview(card.sprite);

        Rect lastRect = GUILayoutUtility.GetLastRect();
        float w = 160f;
        Rect rect = new Rect(10, lastRect.y + lastRect.height + 10, w, (card.sprite.rect.height / card.sprite.rect.width) * w);

        GUI.DrawTexture(rect, textureBust);
    }
}
