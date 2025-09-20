using UnityEngine;

[CreateAssetMenu(menuName = "My File/Game/Cutscene Pack")]
public class CutscenePack : ScriptableObject {

    [SerializeField] DialogueLine[] lines;
    [Space]
    int currentLine;

    public int Line {
        get { return currentLine; }
        set { currentLine = value; }
    }

    public DialogueLine GetLine() {
        return lines[currentLine];
    }
    public bool DialogueDone() {
        return currentLine >= lines.Length;
    }

    public void Refresh() {
        Line = 0;
    }
}
