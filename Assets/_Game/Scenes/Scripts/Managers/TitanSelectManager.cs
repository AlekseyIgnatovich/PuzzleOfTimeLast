using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class TitanSelectManager : Manager
{

    [SerializeField] TitanData titanData;
    [SerializeField] GameObject playButton;
    [SerializeField] TimerController titanTimer;
    [SerializeField] Transform battleLogContent;
    [SerializeField] GameObject defeatedText;
    [Space]
    [SerializeField] TextMeshProUGUI logText;

    private void Start()
    {
        titanTimer.SetNextDate(gameManager.timersManager.nextTitan);
        titanTimer.EnableTimer();
        titanTimer.gameObject.SetActive(true);
        FirebaseManager.instance.firestoreManager.titanOn = true;
        bool defeated = FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.titanDefeated;
        defeatedText.SetActive(defeated);
        playButton.SetActive(!defeated);
        // if (titanData.GetTitanTimer() > 0)
        // {
        //     playButton.SetActive(false);
        //     titanTimer.gameObject.SetActive(true);
        // }
        // else
        // {
        //     FirebaseManager.instance.firestoreManager.titanOn = true;
        //     playButton.SetActive(true);
        //     titanTimer.gameObject.SetActive(false);
        // }

        TextMeshProUGUI _text;

        List<string> _log = titanData.GetTitanSet().GetLog();
        List<float> _damage = titanData.GetTitanSet().GetLogDamage();
        //float lastFightTotalDamage = 0f;
        string userName = FirebaseManager.instance.firestoreManager.playerData.userName;

        Debug.Log("Titan Logs: " + _damage.Count);
        for (int i = 0; i < _damage.Count; i++)
        {
            //lastFightTotalDamage += _damage[i];
            _text = Instantiate(logText, battleLogContent);
            _text.text = userName + " attacked for " + _damage[i] + " Damage!";
        }
        // if (lastFightTotalDamage > 0f)
        // {
        // }
    }

    private void Update()
    {
        if (!playButton.activeSelf)
        {
            // if (titanData.GetTitanTimer() <= 0)
            // {
            //     FirebaseManager.instance.firestoreManager.titanOn = true;
            //     playButton.SetActive(true);
            //     titanTimer.SetActive(false);
            // }
        }

        //TO DO: Replace this call from firebase for a local reference set on Start
        // if (FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.titanDefeated)
        // {
        //     double hoursSinceDefeat = (DateTime.Now - FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.lastTitanDefeatTime).TotalHours;

        //     if (hoursSinceDefeat >= 6)
        //     {
        //         playButton.SetActive(true);
        //         titanTimer.SetActive(false);
        //         FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.titanDefeated = false;
        //         FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
        //     }
        //     else
        //     {
        //         playButton.SetActive(false);
        //         titanTimer.SetActive(true);
        //     }
        // }
    }


    public void UpdateBattleLogsInCloud()
    {
        //TO DO: This Function will update the info in the battleLogs from the cloud
    }

    public void PlayTitanStage()
    {
        gameManager.SetState(GameState.Loadout);
    }

}
