using UnityEngine;

[CreateAssetMenu(menuName = "My File/Game/Dialogue Line")]
public class DialogueLine : ScriptableObject {

    public string actorName;
    public Sprite actorSprite;
    [Space]
    [TextArea(2, 5)]
    public string dialogueLine;

}
