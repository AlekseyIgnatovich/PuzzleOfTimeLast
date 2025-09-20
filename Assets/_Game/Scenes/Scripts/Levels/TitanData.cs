using UnityEngine;

[CreateAssetMenu(menuName = "My File/Titans Data")]
public class TitanData : ScriptableObject
{

    public TitanSet[] titanSet;

    // public int titanSelected;

    [SerializeField] MobCard[] titans;

    public bool once { get; set; }
    public MobCard[] Titans { get { return titans; } }
    public int currentTitanIndex;

    public void UpdateTimers()
    {
        float _tmDelta = Time.deltaTime;

        for (int i = 0; i < titanSet.Length; i++)
        {
            if (titanSet[i].timer > 0) { titanSet[i].timer -= _tmDelta; }
        }
    }

    public void Refresh()
    {
        titanSet[0].Refresh(titans[currentTitanIndex]);
        // for (int i = 0; i < titanSet.Length; i++)
        // {
        // }
        // if (!once)
        // {
        //     once = true;
        //     FirebaseManager.instance.gameManager.notificationsManager.SendPushNotification("Titan Ready!", "Titan is ready!");
        //     FirebaseManager.instance.gameManager.AddNotification("Titan is ready!");
        // }
    }

    public void LoadTitanIndex()
    {
        currentTitanIndex = GameManager.instance.timersManager.ReturnTitanIndex();
        Debug.Log("Current index: " + currentTitanIndex);
    }

    public void LoadFromCloud(CloudPlayerData playerData)
    {
        if (currentTitanIndex != FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.currentTitanIndex)
        {
            FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.currentTitanIndex = currentTitanIndex;
            FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.titanDefeated = false;
            FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.battleLogDamage.Clear();
        }
        titanSet[0].titan = titans[currentTitanIndex];
        titanSet[0].battleLogDamage = playerData.cloudTitanSetData.battleLogDamage;
        titanSet[0].titanHealth = playerData.cloudTitanSetData.titanHealth;
        UpdateTitanHealth();
        Debug.Log("Titan health now is: " + titanSet[0].titanHealth);
        titanSet[0].timer = 0f;
        Debug.Log("BallteLogDamage: " + playerData.cloudTitanSetData.battleLogDamage.Count);
    }
    // public void Load(float _timediffrence)
    // {
    //     for (int i = 0; i < titanSet.Length; i++)
    //     {
    //         titanSet[i].titan = titans[PlayerPrefs.GetInt($"titan{i}", 0)];
    //         titanSet[i].titanHealth = PlayerPrefs.GetFloat($"health{i}", titanSet[i].titan.maxHealth);
    //         titanSet[i].timer = Mathf.Max(0, PlayerPrefs.GetFloat($"timer{i}", 0) - _timediffrence);
    //     }
    // }

    public TitanSet GetTitanSet()
    {
        if ((currentTitanIndex < 0) || (currentTitanIndex >= titans.Length)) return null;

        return titanSet[0];
    }
    public MobCard GetTitan()
    {
        Debug.Log("Geteando Titan: " + titanSet[0].titan);
        return titanSet[0].titan;
    }
    public float GetTitanHealth()
    {
        if ((currentTitanIndex < 0) || (currentTitanIndex >= titans.Length)) return 0;

        UpdateTitanHealth();
        Debug.Log("Titan Health: " + titanSet[0].titanHealth);
        return titanSet[0].titanHealth;
    }
    public float GetTitanMaxHealth()
    {
        if ((currentTitanIndex < 0) || (currentTitanIndex >= titans.Length)) return 0;

        return titanSet[0].titan.maxHealth;
    }
    public float GetTitanTimer()
    {
        if ((currentTitanIndex < 0) || (currentTitanIndex >= titans.Length)) return 0;

        return titanSet[0].timer;
    }

    public void RunTitanTimer()
    {
        titanSet[0].RunTimer();
    }
    public void SaveTitanHealth(float _health)
    {
        //titanSet[0].titanHealth = _health;
        UpdateTitanHealth();
    }

    public void UpdateTitanHealth()
    {
        titanSet[0].titanHealth = titanSet[0].titan.maxHealth;
        for (int i = 0; i < titanSet[0].battleLogDamage.Count; i++)
        {
            titanSet[0].titanHealth -= titanSet[0].battleLogDamage[i];
        }
        FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.titanHealth = titanSet[0].titanHealth;
        //Debug.Log("Titan Health: " + titanSet[0].titanHealth);
    }
    public void RefreshTitan()
    {
        Debug.Log("Refreshing Titan");
        currentTitanIndex++;
        if (currentTitanIndex >= titans.Length) currentTitanIndex = 0;
        titanSet[0].Refresh(titans[currentTitanIndex]);
    }


    public void SaveTitans(bool isOverride = false)
    {
        FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.currentTitanIndex = currentTitanIndex;
        if (!isOverride)
        {
            UpdateTitanHealth();
        }
        FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.titanHealth = titanSet[0].titanHealth;
        FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.battleLogDamage = titanSet[0].battleLogDamage;
        FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.titanDefeated = titanSet[0].titanHealth <= 0;
        // for (int i = 0; i < titanSet.Length; i++)
        // {
        //     PlayerPrefs.SetInt($"titan{i}", GetTitanIndex(titanSet[i].titan));
        //     PlayerPrefs.SetFloat($"health{i}", titanSet[i].titanHealth);
        //     PlayerPrefs.SetFloat($"timer{i}", titanSet[i].timer);
        // }
    }

    int GetTitanIndex(MobCard _titan)
    {
        for (int i = 0; i < titans.Length; i++)
        {
            if (titans[i] == _titan)
            {
                return i;
            }
        }

        return 0;
    }

    public void SelectNextTitan()
    {
        //titanSelected = (titanSelected + 1) % titanSet.Length;
        FirebaseManager.instance.firestoreManager.playerData.cloudTitanSetData.titanDefeated = true;

        //RefreshTitan();
    }
}