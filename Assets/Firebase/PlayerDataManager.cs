using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System;

[FirestoreData]
public struct CloudPlayerData
{
    [FirestoreProperty] public CloudCurrencyData CurrencyData { get { return currencyData; } set { currencyData = value; } }
    [FirestoreProperty] public CloudItemsData ItemsData { get { return itemsData; } set { itemsData = value; } }
    [FirestoreProperty] public CloudHeroesData HeroesData { get { return heroesData; } set { heroesData = value; } }
    [FirestoreProperty] public CloudRewardsData CloudRewardData { get { return rewardData; } set { rewardData = value; } }
    [FirestoreProperty] public CloudGoldenPassData CloudGoldenPassData { get { return goldenPassData; } set { goldenPassData = value; } }
    [FirestoreProperty] public CloudDailyDealData CloudDailyDealData { get { return dailyDealData; } set { dailyDealData = value; } }
    [FirestoreProperty] public CloudMapLevelsData CloudMapLevelsData { get { return mapLevelsData; } set { mapLevelsData = value; } }
    [FirestoreProperty] public CloudTitanSetData CloudTitanSetData { get { return cloudTitanSetData; } set { cloudTitanSetData = value; } }

    // Información básica del usuario
    [FirestoreProperty] public string userName { get; set; }
    [FirestoreProperty] public string userEmail { get; set; }
    [FirestoreProperty] public string userID { get; set; }

    // Fechas clave
    [FirestoreProperty] public DateTime firstLogin { get; set; }
    [FirestoreProperty] public DateTime lastLogin { get; set; }
    [FirestoreProperty] public DateTime lastDailySpinTime { get; set; }

    // Campos existentes
    public CloudCurrencyData currencyData;
    public CloudItemsData itemsData;
    public CloudHeroesData heroesData;
    public CloudRewardsData rewardData;
    public CloudGoldenPassData goldenPassData;
    public CloudDailyDealData dailyDealData;
    public CloudMapLevelsData mapLevelsData;

    public CloudTitanSetData cloudTitanSetData;
}

[FirestoreData]
public struct CloudCurrencyData
{
    [FirestoreProperty] public float coins { get; set; }

    [FirestoreProperty] public float diamonds { get; set; }
}

[FirestoreData]
public struct CloudItemsData
{
    [FirestoreProperty] public string[] itemsID { get; set; }
    [FirestoreProperty] public int[] itemsAmount { get; set; }
}

[FirestoreData]
public struct CloudHeroesData
{
    [FirestoreProperty] public int[] index { get; set; }
    [FirestoreProperty] public int[] level { get; set; }
    [FirestoreProperty] public float[] maxHP { get; set; }
    [FirestoreProperty] public float[] attack { get; set; }
    [FirestoreProperty] public float[] defense { get; set; }
    [FirestoreProperty] public float[] experience { get; set; }
    [FirestoreProperty] public bool[] selected { get; set; }
    [FirestoreProperty] public int[] rarity { get; set; }
    [FirestoreProperty] public int[] type { get; set; }

}

[FirestoreData]
public struct CloudRewardsData
{
    [FirestoreProperty] public int currentDayIndex { get; set; }
    [FirestoreProperty] public int lastDailyRewardIndex { get; set; }
    [FirestoreProperty] public bool[] hasClaimedReward { get; set; }
    [FirestoreProperty] public DateTime lastDayClaimed { get; set; }
    [FirestoreProperty] public bool subscribed { get; set; }
}

[FirestoreData]
public struct CloudDailyDealData
{
    [FirestoreProperty] public bool claimed { get; set; }
    [FirestoreProperty] public int lastDailyRewardIndex { get; set; }
    [FirestoreProperty] public bool[] hasClaimedDeal { get; set; }
}

[FirestoreData]
public struct CloudGoldenPassData
{
    [FirestoreProperty] public int currentPassLevel { get; set; }
    [FirestoreProperty] public bool[] freeClaimed { get; set; }
    [FirestoreProperty] public bool[] premiumClaimed { get; set; }
    [FirestoreProperty] public bool isPremium { get; set; }
}


[FirestoreData]
public struct CloudMapLevelsData
{
    [FirestoreProperty] public List<string> worldName { get; set; }
    [FirestoreProperty] public List<bool> isUnlocked { get; set; }
    [FirestoreProperty] public List<int> lvProgress { get; set; }
    [FirestoreProperty] public List<int> lvSelected { get; set; }

}

[FirestoreData]
public struct CloudTitanSetData
{
    [FirestoreProperty] public int currentTitanIndex { get; set; }
    [FirestoreProperty] public float titanHealth { get; set; }
    [FirestoreProperty] public bool titanDefeated { get; set; }
    [FirestoreProperty] public List<float> battleLogDamage { get; set; }
    [FirestoreProperty] public DateTime lastTitanDefeatTime { get; set; }
    // [FirestoreProperty] public DateTime lastTitanRefreshTime { get; set; }
}

[FirestoreData]
public struct CloudGlobalDateTimes
{
    [FirestoreProperty] public DateTime initialTitanDate { get; set; }
}
