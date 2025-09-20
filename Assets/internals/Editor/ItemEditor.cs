using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemObject))]
public class ItemEditor : Editor {

    ItemObject item;

    private void OnEnable() {
        item = target as ItemObject;
    }

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        //===========================================================================
        Sprite _sprite = item.item.sprite;
        if (_sprite == null) { return; }

        Rect lastRect = GUILayoutUtility.GetLastRect();
        float w = 160f;

        Rect rect = new Rect(10, lastRect.y + lastRect.height + 10, w, (_sprite.rect.height / _sprite.rect.width) * w);

        Texture2D textureBust = AssetPreview.GetAssetPreview(_sprite);

        GUI.DrawTexture(rect, textureBust);
    }
}
