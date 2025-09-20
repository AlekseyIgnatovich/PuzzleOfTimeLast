using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoPanelController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleTMP;
    [SerializeField] TextMeshProUGUI[] itemsTMP;
    [SerializeField] GameObject info;
    [SerializeField] Button infoBtn;

    [SerializeField] InfoPack infoPack;

    bool isShowing;

    void Start()
    {
        // Agregamos los listeners para los eventos PointerDown y PointerUp
        EventTrigger trigger = infoBtn.gameObject.AddComponent<EventTrigger>();

        // Agregar evento PointerDown
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { ShowInfo(true); });
        trigger.triggers.Add(pointerDownEntry);

        // Agregar evento PointerUp
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => { ShowInfo(false); });
        trigger.triggers.Add(pointerUpEntry);

        titleTMP.text = infoPack.titleTxt;
        UpdateItemsInfo(infoPack.itemsTxt);
    }

    void ShowInfo(bool show)
    {
        isShowing = show;
        info.SetActive(isShowing);
    }
    public void UpdateItemsInfo(string[] itemsInfo)
    {
        for (int i = 0; i < itemsTMP.Length; i++)
        {
            itemsTMP[i].text = itemsInfo[i];
        }
    }
}

[System.Serializable]
public class InfoPack
{
    public string titleTxt;
    public string[] itemsTxt;
}