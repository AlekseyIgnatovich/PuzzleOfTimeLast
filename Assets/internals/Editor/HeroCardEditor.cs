using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeroCard))]
public class HeroCardEditor : Editor {

    HeroCard card;

    private void OnEnable() {
        card = target as HeroCard;
    }

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        float w = 160f;
        float h = (card.sprite.rect.height / card.sprite.rect.width) * w;
        Rect lastRect = GUILayoutUtility.GetLastRect();
        Rect rect = new Rect(10, lastRect.y + lastRect.height + 10, w, h);

        //===========================================================================
        if (card.sprite != null) {
            Texture2D textureBust = AssetPreview.GetAssetPreview(card.sprite);

            GUI.DrawTexture(rect, textureBust);
        }

        //===========================================================================
        if (card.cardSprite != null) {
            Texture2D textureCard = AssetPreview.GetAssetPreview(card.cardSprite);

            rect.x += w + 10;
            rect.height = (card.cardSprite.rect.height / card.cardSprite.rect.width) * w;
            GUI.DrawTexture(rect, textureCard);
        }

        //===========================================================================
        if (card.gameSprite == null) { return; }

        Texture2D textureGame = AssetPreview.GetAssetPreview(card.gameSprite);

        rect.x += w + 10;
        rect.height = (card.gameSprite.rect.height / card.gameSprite.rect.width) * w;
        GUI.DrawTexture(rect, textureGame);
    }
}
