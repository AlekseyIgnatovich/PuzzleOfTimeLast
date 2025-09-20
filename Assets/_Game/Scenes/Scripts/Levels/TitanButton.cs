using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitanButton : MonoBehaviour
{

    [SerializeField] TitanData titanData;
    [SerializeField] WorldMapManager manager;
    [SerializeField] TextMeshProUGUI textButton;
    [Space]
    [SerializeField] int levelSelect;
    [SerializeField] int titanSelect;


    private void Update()
    {
        UpdateLabel();
    }

    public void SelectLevel()
    {
        titanSelect = titanData.currentTitanIndex;
        //if (titanData.titanSet[titanSelect].timer > 0) { return; }
        //manager.SelectStage(levelSelect);
        manager.data.SelectLevel(levelSelect);
        manager.gameManager.SetState(GameState.TitanSelect);
    }
    void UpdateLabel()
    {
        float _timer = titanData.titanSet[titanSelect].timer;

        if (_timer > 0)
        {
            float _min = Mathf.Floor(_timer / 60);
            textButton.text = $"{_min}:{Mathf.Floor(_timer - (_min * 60))}";
        }
        else
        {
            textButton.text = titanData.titanSet[titanSelect].titan.Name;
        }
    }
}
