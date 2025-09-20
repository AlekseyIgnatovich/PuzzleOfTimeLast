using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StagePopup : MonoBehaviour {

    [SerializeField] Manager manager;
    [Space]
    [SerializeField] Image stagePicture;
    [SerializeField] TextMeshProUGUI stageName;
    [SerializeField] TextMeshProUGUI stageDescription;

    private void Start() {
        UpdateInfo();
    }

    public void UpdateInfo() {
        Level _level = manager.gameManager.levelsData.GetSelectedLevel();

        stagePicture.sprite = _level.picture;
        stageName.text = _level.Name;
        stageDescription.text = _level.description;
    }
}
