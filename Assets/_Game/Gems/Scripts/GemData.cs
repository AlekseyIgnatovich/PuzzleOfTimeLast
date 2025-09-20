using UnityEngine;

[CreateAssetMenu(menuName ="My File/Gems/Gem Data")]
public class GemData : ScriptableObject {

    public ElementType[] types;
    public Sprite[] sprites;
    public Sprite[] spritesM4;
    public Sprite[] spritesM5;

    public void SetGem(Gem gem, int i = -1) {
        if ((i < 0) || (i >= types.Length)) { i = Random.Range(0, types.Length); }

        gem.Type = types[i];
        gem.spriteRenderer.sprite = GetSprite(types[i]);
    }

    public Sprite GetSprite(ElementType _type) {
        return sprites[(int)_type - 1];
    }
    public Sprite GetSpriteM4(ElementType _type) {
        return spritesM4[(int)_type - 1];
    }
    public Sprite GetSpriteM5(ElementType _type) {
        return spritesM5[(int)_type - 1];
    }
}
