using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    [SerializeField] GameObject lockGO;
    [SerializeField] Button button;
    public UIMapData data;
    public MapSelectManager mapSelectManager;

    public void Initialize(bool _unlocked)
    {
        if (GameManager.instance.isFirstTime)
        {
            button.enabled = data.menuIndex == 0;
        }
        data.isUnlocked = _unlocked;
        //button.enabled = data.isUnlocked;
        lockGO.SetActive(!data.isUnlocked);
    }

    public void OnMapPressed()
    {
        mapSelectManager.currentMap = this;
        if (data.isUnlocked)
        {
            mapSelectManager.ShowPopup(data.menuIndex);
        }
        else
        {
            mapSelectManager.ShowMessage(data.unlockMessage);
        }
    }
}
