using UnityEngine;
using TMPro;

public class NarratorPanel : MonoBehaviour {

    [SerializeField] GameplayManager manager;
    [SerializeField] Transform characterSlot;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textLine;
    [SerializeField] GameObject uiElements;
    [Space]
    [SerializeField] DialogueParty partyPrefab;

    CutscenePack cutscene;
    GameplayState gameplayState;

    DialogueParty lastParty;
    float timer;
    bool cutsceneEnded;

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                manager.SetState(gameplayState);
                gameObject.SetActive(false);
            }
        }
    }

    public void Setup(CutscenePack _pack, GameplayState _gs) {
        gameObject.SetActive(true);
        uiElements.SetActive(true);
        cutsceneEnded = false;

        cutscene = _pack;
        gameplayState = _gs;
        cutscene.Refresh();
        SpawnDialogueLine();
    }

    void SpawnDialogueLine() {
        if (cutsceneEnded) { return; }

        if (cutscene.DialogueDone()) {
            if (lastParty != null) { lastParty.End(); }
            End();
            return;
        }

        DialogueLine _line = cutscene.GetLine();
        Sprite _spr = null;

        if (lastParty != null) { _spr = lastParty.picture.sprite; }

        if (_line.actorSprite != _spr) {
            if (lastParty != null) { lastParty.End(); }
            lastParty = Instantiate(partyPrefab, characterSlot);
            lastParty.Setup(_line);
        }
        textName.text = _line.actorName;
        textLine.text = _line.dialogueLine;

        cutscene.Line++;
    }

    public void NextLine() {
        SpawnDialogueLine();
    }
    public void SkipCutscene() {
        End();
    }

    public void End() {
        if (cutsceneEnded) { return; }

        DialogueParty[] _parties = characterSlot.GetComponentsInChildren<DialogueParty>();
        for (int i = 0; i < _parties.Length; i++) {
            _parties[i].End();
        }
        textName.text = "";
        textLine.text = "";
        uiElements.SetActive(false);

        timer = .3f;
        cutsceneEnded = true;
    }
}
