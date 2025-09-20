using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageButton : MonoBehaviour {

    [SerializeField] WorldMapManager manager;
    [SerializeField] GameObject[] state;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI[] number;
    [Space]
    [SerializeField] bool bypass;
    [SerializeField] int select;
    public int progress;

    private void Start() {
        int _ps = manager.gameManager.levelsData.progress;
        progress = 0;

        if (bypass) {
            progress = 1;
        } else {
            if (select == _ps) { progress = 1; }
            if (select < _ps) { progress = 2; }
            for (int i = 0; i < number.Length; i++) {
                number[i].text = $"{select}";
            }
        }

        for (int i = 0; i < state.Length; i++) {
            state[i].SetActive(progress == i);
        }
    }

    public void SelectLevel() {
        if (progress <= 0) { return; }
        manager.SelectStage(select);
    }

}
