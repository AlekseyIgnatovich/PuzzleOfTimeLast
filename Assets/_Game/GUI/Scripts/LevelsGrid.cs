using UnityEngine;
using UnityEngine.UI;

public class LevelsGrid : MonoBehaviour {

    [SerializeField] LevelSelectManager manager;
    [SerializeField] LevelsDatabase database;
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] LevelButton button;
    [SerializeField] LevelSelectorUI selector;
    [SerializeField] ScrollRect scrollRect;
    [Space]
    [SerializeField] AudioClip levelSelectSFX;

    private void OnEnable() {
        manager.gameManager.levelsData.OnSlectionChange += UpdateSelection;
    }
    private void OnDisable() {
        manager.gameManager.levelsData.OnSlectionChange -= UpdateSelection;
    }

    private void Awake() {
        database = manager.gameManager.levelsData;
        LevelButton btn;
        int l = database.levels.Length;
        for (int i = 0; i < l; i++) {
            btn = Instantiate(button, transform);
            btn.SetButton(database, i);
        }
    }
    private void Start() {
        SetGroupSize();
        database.SelectLevel(0);
    }

    void SetGroupSize() {
        int buttoncount = database.levels.Length;
        float padding = grid.padding.top + grid.padding.bottom;
        float spacing = grid.spacing.y * (buttoncount - 1);
        float cellSize = grid.cellSize.y * buttoncount;

        RectTransform gridRect = grid.GetComponent<RectTransform>();

        gridRect.sizeDelta = new Vector2(gridRect.rect.width, cellSize + spacing + padding);

        grid.GetComponent<RectTransform>().sizeDelta = gridRect.sizeDelta;

        //grid.transform.localPosition = Vector2.down * -600;
    }

    void UpdateSelection() {
        manager.gameManager.audioData.PlaySFX(levelSelectSFX);
        selector.UpdateSelection(transform.GetChild(database.selectedLevel));
    }
}
