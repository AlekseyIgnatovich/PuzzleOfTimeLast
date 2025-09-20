using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem.Utilities;

public class MapSelectManager : Manager
{

    [SerializeField] GameObject popup;
    [SerializeField] TextMeshProUGUI popupText;
    [SerializeField] UIMap[] worldMaps;
    [SerializeField] GameObject instruction;

    [HideInInspector] public int selectedMap;
    [HideInInspector] public UIMap currentMap;
    private void Start()
    {
        GameManager.onMapFinished += UnlockNextMap;

        if (gameManager.isFirstTime)
        {
            instruction.SetActive(true);
        }
    }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0) && isShowingTutorial)
    //     {
    //         isShowingTutorial = false;
    //         LeanTween.cancelAll();
    //         handImg.gameObject.SetActive(false);
    //     }
    // }


    // private void Update()
    // {
    //     if (!isShowingTutorial) return;

    //     if(onBoardingBG.color.a < 1){
    //         float alpha = 0;
    //     }
    // }



    private void OnDestroy()
    {
        GameManager.onMapFinished -= UnlockNextMap;
    }



    public void ShowPopup(int _selection)
    {
        selectedMap = _selection;
        popupText.text = $"Enter {gameManager.mapsData.levelDatas[selectedMap].mapName} world?";
        popup.SetActive(true);
    }

    public void ShowMessage(string message)
    {
        popupText.text = message;
        popup.SetActive(true);
    }

    public void PopupBtnAction()
    {
        if (currentMap.data.isUnlocked)
        {
            EnterWorld();
        }
        else
        {
            popup.SetActive(false);
        }
    }

    public void EnterWorld()
    {
        gameManager.mapsData.SelectMap(selectedMap);
        gameManager.SetState(GameState.WorldMap);
    }

    public void OnEnable()
    {
        List<bool> mapsUnlocked = FirebaseManager.instance.firestoreManager.playerData.mapLevelsData.isUnlocked;
        for (int i = 0; i < worldMaps.Length; i++)
        {
            bool hasBeenSaved = (i < mapsUnlocked.Count);
            worldMaps[i].Initialize(hasBeenSaved);
        }
    }

    public void UnlockNextMap(string name)
    {
        if (name == gameManager.mapsData.levelDatas[selectedMap].mapName)
        {
            var nextIndex = selectedMap++;
            if (nextIndex >= worldMaps.Length) return;
            worldMaps[nextIndex].data.isUnlocked = true;
        }
    }
}

[System.Serializable]
public class UIMapData
{
    public bool isUnlocked;
    public string unlockMessage;
    public LevelsDatabase levelsData;
    public int menuIndex;
}
