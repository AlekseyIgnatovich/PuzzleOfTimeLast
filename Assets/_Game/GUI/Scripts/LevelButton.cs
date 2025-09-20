using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour {

    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textWaves;
    [SerializeField] TextMeshProUGUI textDescription;

    LevelsDatabase database;

    int number;

    public void SetButton(LevelsDatabase _data, int _number) {
        database = _data;
        number = _number;
        Level _lv = _data.GetLevel(number);
        textName.text = _lv.Name;
        textWaves.text = $"Waves: {_lv.waves.Length}";
        textDescription.text = $"Description: {_lv.description}";
    }

    public void SelectLevel() {
        database.SelectLevel(number);
    }
}
