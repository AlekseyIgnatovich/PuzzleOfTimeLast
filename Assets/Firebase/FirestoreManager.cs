using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Data.Common;
using System.Threading.Tasks;
using System;
using Unity.Services.Analytics;

public class FirestoreManager : MonoBehaviour
{
    public TitanData titanData;
    FirebaseFirestore db;
    ListenerRegistration listenerRegistration;
    ListenerRegistration globalListenerRegistration;
    DocumentReference userDataRef;
    public CloudPlayerData playerData = new CloudPlayerData();
    public Dictionary<string, CloudPlayerData> playerDictionary = new Dictionary<string, CloudPlayerData>();
    public CloudGlobalDateTimes globalDates;

    public bool titanOn, rewardOn, dailyDealOn, dailySpinOn, goldenPassOn;

    public int currentMapIndex = 0;
    public int levelsCompletedInSession = 0;
    int sessionsPlayed = 0;

    private void OnEnable()
    {
        GameManager.onLevelFinished += ModifyCurrentLevel;
        GameManager.onMapFinished += UnlockNextMap;
    }

    private void OnDisable()
    {
        GameManager.onLevelFinished -= ModifyCurrentLevel;
        GameManager.onMapFinished -= UnlockNextMap;
    }
    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void UpdatePlayerDatabase()
    {
        playerDictionary[playerData.userID] = playerData;
        userDataRef = db.Collection("PlayersData").Document(playerData.userEmail);
        userDataRef.SetAsync(playerDictionary).ContinueWithOnMainThread(task =>
            {
                Debug.Log("User Data updated");
            });
    }

    public void CreateUserData(string _mail, string _id, string _name)
    {
        playerData = new CloudPlayerData();
        playerData.userEmail = _mail;
        playerData.userID = _id;
        playerData.userName = _name;
        playerData.firstLogin = DateTime.Now;

        int initialTitanIndex = GameManager.instance.timersManager.ReturnTitanIndex();
        playerData.cloudTitanSetData.currentTitanIndex = initialTitanIndex;
        playerData.cloudTitanSetData.titanHealth = titanData.Titans[initialTitanIndex].maxHealth;
        playerData.cloudTitanSetData.battleLogDamage = new List<float>();
        playerData.cloudTitanSetData.titanDefeated = false;
        playerData.cloudTitanSetData.lastTitanDefeatTime = DateTime.MinValue;
        //playerData.cloudTitanSetData.lastTitanRefreshTime = DateTime.Now;

        playerData.goldenPassData.currentPassLevel = 0;
        playerData.goldenPassData.isPremium = false;
        playerData.goldenPassData.freeClaimed = new bool[FirebaseManager.instance.gameManager.goldenPassManager.freeRewardsAmount];
        playerData.goldenPassData.premiumClaimed = new bool[FirebaseManager.instance.gameManager.goldenPassManager.premiumRewardsAmount];

        playerData.rewardData.hasClaimedReward = new bool[FirebaseManager.instance.gameManager.rewardManager.dailyRewards.Length];
        playerData.rewardData.lastDayClaimed = DateTime.MinValue;
        playerData.dailyDealData.hasClaimedDeal = new bool[FirebaseManager.instance.gameManager.rewardManager.dailyRewards.Length];
        playerData.currencyData.coins = 0;
        playerData.currencyData.diamonds = 0;

        playerData.lastLogin = DateTime.Now;
        playerData.lastDailySpinTime = DateTime.MinValue;
        playerData.mapLevelsData = CreateFirstMapData(FirebaseManager.instance.gameManager.levelsData.mapName);
        UpdateSaveItems(FirebaseManager.instance.gameManager.LoadDefaultItems());
        UpdateSavedHeroes(FirebaseManager.instance.gameManager.LoadDefaultHeroesData());

        // Guardar en Firestore
        playerDictionary.Add(_id, playerData);
        UpdatePlayerDatabase();
    }

    public void LoadPlayerDataFromCloud(string _mail, string _id)
    {
        // Validar db
        if (db == null)
        {
            Debug.LogError("Firestore no está inicializado (db es null).");
            return;
        }

        globalListenerRegistration = db.Collection("GlobalData").Document("GlobalDateTimes").Listen(snapshot =>
        {
            if (snapshot == null)
            {
                Debug.LogError("El snapshot recibido es null.");
                return;
            }
            try
            {
                globalDates = snapshot.ConvertTo<CloudGlobalDateTimes>();
                Debug.Log("La initial date de la nube es: " + globalDates.initialTitanDate);
                GameManager.instance.timersManager.initialTitanDate = globalDates.initialTitanDate;
                GameManager.instance.titansData.LoadTitanIndex();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ocurrió un error procesando los datos del snapshot: {ex.Message}\n{ex.StackTrace}");
            }
        });

        // Escuchar cambios en el documento especificado
        listenerRegistration = db.Collection("PlayersData").Document(_mail).Listen(snapshot =>
        {
            if (snapshot == null)
            {
                Debug.LogError("El snapshot recibido es null.");
                return;
            }

            if (!snapshot.Exists)
            {
                Debug.LogError($"El documento con mail {_mail} no existe en Firestore.");
                return;
            }

            try
            {
                playerDictionary = snapshot.ConvertTo<Dictionary<string, CloudPlayerData>>();

                if (!playerDictionary.ContainsKey(_id))
                {
                    Debug.LogError($"El ID {_id} no está presente en el diccionario.");
                    return;
                }

                playerData = playerDictionary[_id];

                // Cargar los datos del jugador en el GameManager
                var gameManager = FirebaseManager.instance?.gameManager;
                if (gameManager == null)
                {
                    Debug.LogError("GameManager es null. Asegúrate de que esté inicializado correctamente.");
                    return;
                }

                gameManager.LoadCurrencies(playerData.currencyData.coins, playerData.currencyData.diamonds);
                gameManager.LoadItemsData(playerData.itemsData.itemsID, playerData.itemsData.itemsAmount);
                gameManager.LoadHeroesData(playerData);
                gameManager.LoadCurrentLevel(playerData.mapLevelsData);

                FirebaseManager.onUserLogin?.Invoke();
                gameManager.titansData.LoadFromCloud(playerData);
                titanOn = titanData.GetTitanTimer() <= 0;
                if (titanOn)
                {
                    var titleManager = gameManager.currentScreen?.GetComponent<TitleManager>();
                    if (titleManager != null)
                    {
                        titleManager.ActivateTitanBtnNotification();
                    }
                    else
                    {
                        Debug.Log("Not currently on title screen.");
                    }
                }

                int currentPassIndex = playerData.goldenPassData.currentPassLevel;
                if (currentPassIndex >= playerData.goldenPassData.premiumClaimed.Length)
                {
                    currentPassIndex = playerData.goldenPassData.premiumClaimed.Length - 1;
                }

                goldenPassOn = !playerData.goldenPassData.premiumClaimed[currentPassIndex] ||
                               !playerData.goldenPassData.freeClaimed[currentPassIndex];

                if (goldenPassOn)
                {
                    var titleManager = gameManager.currentScreen?.GetComponent<TitleManager>();
                    if (titleManager != null)
                    {
                        titleManager.ActivateGoldenPassNotification();
                    }
                    else
                    {
                        Debug.Log("Not currently on title screen.");
                    }
                }

                var goldenPassManager = GameManager.instance?.goldenPassManager;
                if (goldenPassManager != null)
                {
                    goldenPassManager.Initialize(playerData.goldenPassData.isPremium);
                }
                else
                {
                    Debug.LogError("GoldenPassManager es null.");
                }

                FirebaseManager.onHomeScreenShow?.Invoke();

                var loadingScreen = FirebaseManager.instance?.loadingScreen;
                if (loadingScreen != null && loadingScreen.isLoading)
                {
                    loadingScreen.AddAmountToFill(50);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ocurrió un error procesando los datos del snapshot: {ex.Message}\n{ex.StackTrace}");
            }
        });
    }

    public void UpdateTitanData()
    {

    }

    public void UpdateSaveItems(SaveItem[] newItems)
    {
        playerData.itemsData.itemsAmount = new int[newItems.Length];
        playerData.itemsData.itemsID = new string[newItems.Length];

        for (int i = 0; i < newItems.Length; i++)
        {
            playerData.itemsData.itemsAmount[i] = newItems[i].itemAmount;
            playerData.itemsData.itemsID[i] = newItems[i].itemID;
        }
    }

    public void UpdateSavedHeroes(HeroItem[] heroItems)
    {
        playerData.heroesData = new CloudHeroesData();
        playerData.heroesData.index = new int[heroItems.Length];
        playerData.heroesData.level = new int[heroItems.Length];
        playerData.heroesData.maxHP = new float[heroItems.Length];
        playerData.heroesData.attack = new float[heroItems.Length];
        playerData.heroesData.defense = new float[heroItems.Length];
        playerData.heroesData.experience = new float[heroItems.Length];
        playerData.heroesData.selected = new bool[heroItems.Length];
        playerData.heroesData.type = new int[heroItems.Length];
        playerData.heroesData.rarity = new int[heroItems.Length];
        for (int i = 0; i < heroItems.Length; i++)
        {
            playerData.heroesData.index[i] = heroItems[i].index;
            playerData.heroesData.level[i] = heroItems[i].level;
            playerData.heroesData.maxHP[i] = heroItems[i].maxHealth;
            playerData.heroesData.attack[i] = heroItems[i].attack;
            playerData.heroesData.defense[i] = heroItems[i].defense;
            playerData.heroesData.experience[i] = heroItems[i].experience;
            playerData.heroesData.selected[i] = heroItems[i].selected;
            playerData.heroesData.type[i] = (int)heroItems[i].type;
            playerData.heroesData.rarity[i] = (int)heroItems[i].rarity;
        }
    }

    public CloudMapLevelsData CreateFirstMapData(string _name)
    {
        CloudMapLevelsData mapsData = new CloudMapLevelsData();
        mapsData.worldName = new List<string>();
        mapsData.isUnlocked = new List<bool>();
        mapsData.isUnlocked.Add(true);
        mapsData.worldName.Add(_name);
        mapsData.lvProgress = new List<int>();
        mapsData.lvProgress.Add(0);
        mapsData.lvSelected = new List<int>();
        mapsData.lvSelected.Add(0);
        return mapsData;
    }

    public float GetPlayerCoins()
    {
        return playerData.currencyData.coins;
    }
    public float GetPlayerDiamonds() => playerData.currencyData.diamonds;

    public bool ModifyCoinsAmount(float _amount)
    {
        if (_amount < 0 && playerData.currencyData.coins - _amount < 0)
        {
            return false;
        }
        else
        {
            playerData.currencyData.coins += _amount;
            UpdatePlayerDatabase();
            return true;
        }

    }
    public bool ModifyDiamondsAmount(float _amount)
    {
        Debug.Log("amount" + _amount);
        if (_amount < 0 && (playerData.currencyData.diamonds - _amount) < 0)
        {
            return false;
        }
        else
        {
            playerData.currencyData.diamonds += _amount;
            UpdatePlayerDatabase();
            return true;
        }
    }

    public void ModifyCurrentLevel(string _mapName, int _newLevel)
    {
        bool hasFoundMap = false;

        for (int i = 0; i < playerData.mapLevelsData.worldName.Count; i++)
        {
            if (playerData.mapLevelsData.worldName[i] == _mapName)
            {
                if (_newLevel > playerData.mapLevelsData.lvProgress[i])
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {
                        { "world_name", _mapName },
                        { "highest_level", _newLevel }
                    };
                    AnalyticsService.Instance.CustomData("highest_level_reached", parameters);

                    levelsCompletedInSession++;  // Aumenta el contador de niveles completados en esta sesión
                }

                playerData.mapLevelsData.lvProgress[i] = _newLevel;
                hasFoundMap = true;
                break;
            }
        }

        if (!hasFoundMap)
        {
            playerData.mapLevelsData.worldName.Add(_mapName);
            playerData.mapLevelsData.lvProgress.Add(_newLevel);
            playerData.mapLevelsData.lvSelected.Add(_newLevel);

            Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "world_name", _mapName },
                { "highest_level", _newLevel }
            };
            AnalyticsService.Instance.CustomData("highest_level_reached", parameters);

            levelsCompletedInSession++;  // Aumenta el contador de niveles completados en esta sesión
        }

        UpdatePlayerDatabase();
    }



    public void UnlockNextMap(string mapName)
    {
        Debug.Log("New Map Unlocked");
        playerData.mapLevelsData.isUnlocked.Add(true);
        UpdatePlayerDatabase();
    }

    private void OnApplicationQuit()
    {
        TimeSpan timeDifference = DateTime.Now - playerData.lastLogin;

        double timeInSeconds = timeDifference.TotalSeconds;
        Dictionary<string, object> parameters = new Dictionary<string, object>() {
        { "global_time", timeInSeconds },
    };

        AnalyticsService.Instance.CustomData("stage_play_time", parameters);
        EndSession();
    }

    void EndSession()
    {
        sessionsPlayed++;
        float averageLevelsPerSession = (float)levelsCompletedInSession / sessionsPlayed;

        // Enviar el evento de promedio de niveles completados por sesión
        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            { "average_levels_per_session", averageLevelsPerSession }
        };
        AnalyticsService.Instance.CustomData("average_levels_per_session", parameters);

        // Reiniciar el contador de niveles completados para la siguiente sesión
        levelsCompletedInSession = 0;
    }

    public void CheckTitanStatus()
    {
        //TO DO: Make a new verification this one sucks
        // DateTime now = DateTime.Now;

        // if ((now - playerData.cloudTitanSetData.lastTitanRefreshTime).TotalHours >= 24 || playerData.cloudTitanSetData.titanDefeated)
        // {
        //     if (playerData.cloudTitanSetData.titanDefeated)
        //     {
        //         // Revisar cooldown
        //         if ((now - playerData.cloudTitanSetData.lastTitanDefeatTime).TotalHours >= 6)
        //         {
        //             titanData.SelectNextTitan();
        //             playerData.cloudTitanSetData.titanDefeated = false;
        //             playerData.cloudTitanSetData.lastTitanRefreshTime = now;
        //             UpdatePlayerDatabase();
        //         }
        //     }
        //     else
        //     {
        //         titanData.RefreshTitan();
        //         playerData.cloudTitanSetData.lastTitanRefreshTime = now;
        //         UpdatePlayerDatabase();
        //     }
        // }
    }
}
