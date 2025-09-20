using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq.Expressions;
using System.Collections;

[CreateAssetMenu(menuName = "My File/Heroes/Hero Base")]
public class HeroBase : ScriptableObject
{

    public string username = "Jack";
    [Space]
    public GameSettings settings;
    [Space]
    [SerializeField] Sprite[] heroFullCard;
    [SerializeField] Sprite[] heroFrames;
    [SerializeField] Sprite[] heroLabels;
    [Space]
    [Header("All availible heroes")]
    public HeroCard[] heroCards;
    [Header("Default heroes in inventory")]
    public int[] heroDefaultStored;
    [Header("Default heroes")]
    public int[] heroDefault;

    [Space]
    public HeroItem[] hero;
    public int selectedHero;
    [Space]
    public float coins;
    public float diamonds;
    [Space]
    [Header("SETTINGS")]
    [Space]
    public int maxHeroes;
    [Range(0f, 1f)]
    public float specialPercentMod;
    public float specialFlatMod;

    public event Action OnCoinsChange;
    public event Action OnDiamondsChange;
    public event Action OnHeroSelect;
    public event Action OnHeroToggle;

    public void AddCardToInventory(HeroCard _card)
    {
        HeroItem[] _newstore = new HeroItem[hero.Length + 1];

        for (int i = 0; i < hero.Length; i++)
        {
            HeroItem hs = hero[i];
            _newstore[i] = new HeroItem();
            _newstore[i].SetData(hs.index, hs.maxHealth, hs.attack, hs.defense, hs.type, hs.rarity, hs.level, hs.experience, hs.specialMaxPoints, hs.specialPercent, hs.selected);
        }
        _newstore[_newstore.Length - 1] = new HeroItem();
        _newstore[_newstore.Length - 1].SetData(GetCardIndex(_card), _card.maxHealth, _card.attack, _card.defense, _card.type, _card.rarity, 0, 0, _card.specialMaxPoints, _card.specialPercent, false);

        hero = _newstore;

        Debug.Log($"Added {_card.Name} card to inventory");

        FirebaseManager.instance.firestoreManager.UpdateSavedHeroes(hero);
        SaveLocalHeroes();
        Save();
        FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
    }
    public void RefreshInventory()
    {
        int _length = 0;
        for (int j = 0; j < hero.Length; j++)
        {
            _length += hero[j] == null ? 0 : 1;
        }
        HeroItem[] _newstore = new HeroItem[_length];

        HeroItem hs;
        int c = 0;
        for (int i = 0; i < hero.Length; i++)
        {
            hs = hero[i];
            if (hs != null)
            {
                _newstore[c] = new HeroItem();
                _newstore[c].SetData(hs.index, hs.maxHealth, hs.attack, hs.defense, hs.type, hs.rarity, hs.level, hs.experience, hs.specialMaxPoints, hs.specialPercent, hs.selected);
                c++;
            }
        }

        hero = _newstore;

        Debug.Log($"Inventory Refreshed");
    }

    public bool ModifyCoins(float _amount)
    {
        bool answer = FirebaseManager.instance.firestoreManager.ModifyCoinsAmount(_amount);
        if (answer)
        {
            coins += _amount;
            OnCoinsChange?.Invoke();
        }
        else
        {
            GameManager.instance.errorPopup.SetActive(true);
        }
        return answer;
    }
    public bool ModifyDiamonds(float _amount)
    {
        bool answer = FirebaseManager.instance.firestoreManager.ModifyDiamondsAmount(_amount);
        if (answer)
        {
            diamonds += _amount;
            OnDiamondsChange?.Invoke();
        }
        else
        {
            GameManager.instance.errorPopup.SetActive(true);
        }
        return answer;
    }

    public void SelectHero(int _index)
    {
        selectedHero = _index;
        OnHeroSelect?.Invoke();
    }
    public void ToggleHero()
    {
        OnHeroToggle?.Invoke();
    }

    public void Save(int sel = 0)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        if ((sel == 0) || (sel == 2))
        {//save heroes
            formatter.Serialize(stream, hero);
        }

        stream.Close();

        Debug.Log($"Saved heroes data to file");
        int[] heroIndex = new int[hero.Length];
        int[] heroLevel = new int[hero.Length];
        float[] heroMaxHP = new float[hero.Length];
        float[] heroAttack = new float[hero.Length];
        float[] heroDefense = new float[hero.Length];
        float[] heroExperience = new float[hero.Length];
        bool[] heroSelected = new bool[hero.Length];
        int[] heroRarity = new int[hero.Length];
        int[] heroType = new int[hero.Length];
        for (int i = 0; i < hero.Length; i++)
        {
            heroIndex[i] = hero[i].index;
            heroLevel[i] = hero[i].level;
            heroMaxHP[i] = hero[i].maxHealth;
            heroAttack[i] = hero[i].attack;
            heroDefense[i] = hero[i].defense;
            heroExperience[i] = hero[i].experience;
            heroSelected[i] = hero[i].selected;
            heroRarity[i] = (int)hero[i].rarity;
            heroType[i] = (int)hero[i].type;
        }

        CloudHeroesData _cloudHeroesData = FirebaseManager.instance.firestoreManager.playerData.heroesData;

        _cloudHeroesData.index = heroIndex;
        _cloudHeroesData.level = heroLevel;
        _cloudHeroesData.maxHP = heroMaxHP;
        _cloudHeroesData.attack = heroAttack;
        _cloudHeroesData.defense = heroDefense;
        _cloudHeroesData.experience = heroExperience;
        _cloudHeroesData.selected = heroSelected;
        _cloudHeroesData.rarity = heroRarity;
        _cloudHeroesData.type = heroType;
        FirebaseManager.instance.firestoreManager.playerData.heroesData = _cloudHeroesData;
        FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
    }

    public void Load(int _sv)
    {
        string path = Application.persistentDataPath + "/player.bin";

        if (File.Exists(path) && (_sv == settings.saveVersion))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            hero = formatter.Deserialize(stream) as HeroItem[];

            stream.Close();
            Debug.Log($"Loaded heroes from file");
        }
        else
        {
            hero = new HeroItem[heroDefault.Length];

            //Debug.Log($"Loading heroes from cards ]====================================");

            HeroCard hd;
            for (int i = 0; i < hero.Length; i++)
            {
                hd = heroCards[heroDefault[i]];
                //Debug.Log($"Card[{i}]: {hd.Name} Index[{heroDefault[i]}]");
                hero[i] = new HeroItem();
                hero[i].SetData(heroDefault[i], hd.maxHealth, hd.attack, hd.defense, hd.type, hd.rarity, 0, 0, hd.specialMaxPoints, hd.specialPercent, false);
            }
            Debug.Log($"Loaded hero defaults");

            Save();
        }
    }

    public void LoadCloudHeroes(CloudHeroesData _cloudHeroes)
    {
        // HeroItem[] cloudHeroes = new HeroItem[_index.Length];
        // for (int i = 0; i < cloudHeroes.Length; i++)
        // {
        //     cloudHeroes[i].index = _index[i];
        //     cloudHeroes[i].level = _level[i];
        // }
        //hero = new HeroItem[FirebaseManager.instance.firestoreManager.playerData.heroesData.index.Length];
        username = FirebaseManager.instance.firestoreManager.playerData.userName;
        // Debug.Log("The hero is called: " + username);

        HeroCard hd;
        for (int i = 0; i < hero.Length; i++)
        {
            //Debug.Log("i: " + i);
            hd = heroCards[FirebaseManager.instance.firestoreManager.playerData.heroesData.index[i]];
            hero[i] = new HeroItem();
            hero[i].SetData(_cloudHeroes.index[i], _cloudHeroes.maxHP[i], _cloudHeroes.attack[i], _cloudHeroes.defense[i], (ElementType)_cloudHeroes.type[i], (Rarity)_cloudHeroes.rarity[i], _cloudHeroes.level[i], _cloudHeroes.experience[i], hd.specialMaxPoints, hd.specialPercent, _cloudHeroes.selected[i]);
        }
    }

    public void SaveLocalHeroes()
    {
        for (int i = 0; i < hero.Length; i++)
        {
            PlayerPrefs.SetInt($"HeroIndex_{i}", hero[i].index);
            PlayerPrefs.SetInt($"HeroLevel_{i}", hero[i].level);
            PlayerPrefs.SetFloat($"HeroMaxHP_{i}", hero[i].maxHealth);
            PlayerPrefs.SetFloat($"HeroAttack_{i}", hero[i].attack);
            PlayerPrefs.SetFloat($"HeroDefense_{i}", hero[i].defense);
            PlayerPrefs.SetFloat($"HeroExperience_{i}", hero[i].experience);
            PlayerPrefs.SetInt($"HeroSelected_{i}", hero[i].selected ? 1 : 0);
        }
        PlayerPrefs.SetInt("HeroCount", hero.Length);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Diamonds", diamonds);
        PlayerPrefs.Save();
        Debug.Log("Saved heroes data to PlayerPrefs");
    }

    public void LoadLocalHeroes()
    {
        if (!PlayerPrefs.HasKey("HeroCount"))
        {
            LoadDefaultHeroes();
            return;
        }
        int heroCount = PlayerPrefs.GetInt("HeroCount", 0);
        hero = new HeroItem[heroCount];

        for (int i = 0; i < heroCount; i++)
        {
            hero[i] = new HeroItem();
            hero[i].SetData(
                PlayerPrefs.GetInt($"HeroIndex_{i}"),
                PlayerPrefs.GetFloat($"HeroMaxHP_{i}"),
                PlayerPrefs.GetFloat($"HeroAttack_{i}"),
                PlayerPrefs.GetFloat($"HeroDefense_{i}"),
                (ElementType)0,  // No tenemos información del tipo
                (Rarity)0,        // No tenemos información de la rareza
                PlayerPrefs.GetInt($"HeroLevel_{i}"),
                PlayerPrefs.GetFloat($"HeroExperience_{i}"),
                0, 0,
                PlayerPrefs.GetInt($"HeroSelected_{i}") == 1
            );
        }
    }

    // public HeroItem[] LoadDefaultHeroes()
    // {
    //     hero = new HeroItem[heroDefault.Length];

    //     //Debug.Log($"Loading heroes from cards ]====================================");

    //     HeroCard hd;
    //     for (int i = 0; i < hero.Length; i++)
    //     {
    //         hd = heroCards[heroDefault[i]];
    //         //Debug.Log($"Card[{i}]: {hd.Name} Index[{heroDefault[i]}]");
    //         hero[i] = new HeroItem();
    //         hero[i].SetData(heroDefault[i], hd.maxHealth, hd.attack, hd.defense, hd.type, hd.rarity, 0, 0, hd.specialMaxPoints, hd.specialPercent, false);
    //     }
    //     Debug.Log($"Loaded hero defaults");
    //     return hero;
    // }

    public HeroItem[] LoadDefaultHeroes()
    {
        // CloudHeroesData data = new CloudHeroesData();
        // data.index = new int[heroDefault.Length];
        // data.level = new int[heroDefault.Length];

        hero = new HeroItem[heroDefault.Length];

        //Debug.Log($"Loading heroes from cards ]====================================");

        HeroCard hd;
        for (int i = 0; i < hero.Length; i++)
        {
            hd = heroCards[heroDefault[i]];
            //Debug.Log($"Card[{i}]: {hd.Name} Index[{heroDefault[i]}]");
            // data.index[i] = heroDefault[i];
            // data.level[i] = 0;
            hero[i] = new HeroItem();
            hero[i].SetData(heroDefault[i], hd.maxHealth, hd.attack, hd.defense, hd.type, hd.rarity, 0, 0, hd.specialMaxPoints, hd.specialPercent, false);
        }
        Debug.Log($"Loaded hero defaults");
        return hero;
    }

    public void LoadCurrencies(float _coins, float _diamonds)
    {
        //coins = PlayerPrefs.GetFloat("coins", 0);
        //diamonds = PlayerPrefs.GetFloat("diamonds", 0);
        coins = _coins;
        diamonds = _diamonds;
        // Debug.Log("Coins: " + coins);
        // Debug.Log("Diamonds: " + diamonds);
        OnCoinsChange?.Invoke();
        OnDiamondsChange?.Invoke();
        // Debug.Log($"Loaded currencies from file");
    }

    public void DeleteSave()
    {
        string path = Application.persistentDataPath + "/player.bin";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Heroes save deleted");
        }
    }

    public bool SelectForBattle(int _index, bool _select)
    {

        if (_select)
        {
            int _count = 0;
            for (int i = 0; i < hero.Length; i++)
            {
                if ((hero[i] != null) && (hero[i].selected))
                {
                    _count++;
                    if (_count >= maxHeroes)
                    {
                        return false;
                    }
                    else if (_count == (maxHeroes - 1))
                    {
                        //Debug.Log("Return to LoadoutScreen");
                        GameManager.instance.GoToScreen("loadout");
                    }
                }
            }
        }

        hero[_index].selected = _select;

        Save();

        return true;
    }
    public int GetBattleReadyHeroes()
    {
        int _count = 0;
        for (int i = 0; i < hero.Length; i++)
        {
            if ((hero[i] != null) && (hero[i].selected))
            {
                _count++;
            }
        }
        return _count;
    }
    public HeroItem GetHeroInOrder(int _index)
    {
        int _count = 0;
        for (int i = 0; i < hero.Length; i++)
        {
            if ((hero[i] != null) && (hero[i].selected))
            {
                if (_count == _index)
                {
                    return hero[i];
                }
                _count++;
            }
        }
        return null;
    }

    public int GetCardIndex(HeroCard _card)
    {
        for (int i = 0; i < heroCards.Length; i++)
        {
            if (_card == heroCards[i]) { return i; }
        }
        return -1;
    }
    public HeroItem GetSelectedHero()
    {
        return hero[selectedHero];
    }
    public HeroCard GetSelectedCard()
    {
        return heroCards[hero[selectedHero].index];
    }

    public Sprite GetTierCard(Rarity _index)
    {
        return heroFullCard[(int)_index];
    }
    public Sprite GetElementFrame(ElementType _index)
    {
        return heroFrames[(int)_index];
    }
    public Sprite GetElementLabel(ElementType _index)
    {
        return heroLabels[(int)_index];
    }

    public float SpecialMod(float _spc, float _maxsp)
    {
        return specialPercentMod > 0 ? _spc * (_maxsp * specialPercentMod) : _spc * specialFlatMod;
    }

    public HeroCard GetRandomCard(Rarity _rarity)
    {
        HeroCard _randHero = RandomizeHero();
        if (_randHero.rarity == _rarity)
        {
            return _randHero;
        }
        else
        {
            return null;
        }
    }

    public HeroCard RandomizeHero()
    {
        int _rand = Mathf.RoundToInt(UnityEngine.Random.Range(0, heroCards.Length));
        HeroCard _hero = heroCards[_rand];
        return _hero;
    }
}
