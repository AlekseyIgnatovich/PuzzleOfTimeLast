using UnityEngine;
using TMPro;

public class LoadoutUI : MonoBehaviour {

    [SerializeField] LevelsDatabase data;
    [SerializeField] TextMeshProUGUI textLevel;

    private void OnEnable() {
        textLevel.text = $"Level {data.selectedLevel}";
    }
}
