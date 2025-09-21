using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] public bool showTutorial;
    public MapsData mapsData;
    [HideInInspector] public LevelsDatabase levelsData;
    public TitanData titansData;
    [Space]
    public GameState state;
    public AudioData audioData;
    //public NotificationsManager notificationsManager;
    [Space]
    public Manager currentScreen;
    [Space]
    [SerializeField] Manager titleScreen;
    [SerializeField] Manager mainMenuScreen;
    [SerializeField] Manager inventoryScreen;
    [SerializeField] Manager itemsScreen;
    [SerializeField] Manager worldMapScreen;
    [SerializeField] Manager levelSelectScreen;
    [SerializeField] Manager gameplayScreen;
    [SerializeField] Manager shopScreen;
    [SerializeField] Manager settingsScreen;
    [SerializeField] Manager levelupScreen;
    [SerializeField] Manager loadoutScreen;
    [SerializeField] Manager heroSelectScreen;
    [SerializeField] Manager itemSelectScreen;
    [SerializeField] Manager diamondsShopScreen;
    [SerializeField] Manager heroUnlockScreen;
    [SerializeField] Manager mapSelectScreen;
    [SerializeField] Manager titanSelectScreen;
    [SerializeField] GameObject HeroSummonScreen;
    [SerializeField] HeroAnimationController heroAnimScreen;
    [Space]
    [SerializeField] AllItems allItems;
    [SerializeField] HeroBase heroBase;
    [SerializeField] ItemsData itemsData;
    [Space]
    public Canvas canvas;
    [SerializeField] InfoPopup infoPop;
    [Space]
    [SerializeField] MenuButton[] menuButton;
    [SerializeField] ObjectsPack[] screenPacks;
    [SerializeField] GameSettings settings;
    [SerializeField] GameObject notification;
    [SerializeField] ColorTypeResistances[] colorTypeResistances;
    [SerializeField] KickstarterBtnController kickstarterBtnController;
    [SerializeField] GameObject splashScreen;
    [SerializeField] bool showSplashLogo;
    public GameObject errorPopup;
    public BottomTabController bottomTabController;
    public ObjectsPack menuPack;
    public GameObject bottomTabObj;
    public TimersManager timersManager;
    public RewardManager rewardManager;
    public GoldenPassManager goldenPassManager;
    public OnBoardingManager onBoardingManager;
    public SubscriptionManager subscriptionManager;
    public UniversalMenuController universalMenuController;
    public DailySpin dailySpin;

    public float titanTimer;
    public float titanTimerSet;
    public bool goldenPassBtnPressed, rwrdBtnPressed, dailyDealBtnPressed, titanBtnPressed;
    float timer;
    [Space]
    [SerializeField] TextMeshProUGUI mapName;
    public CurrencyPanelUI currencyPanelUI;

    [Space(10)]
    [Header("TUTORIAL BOOLEANS")]
    //For all the initial menu tutorials
    public bool isFirstTime;
    //First hero special activation
    public bool isFirstSpecial;
    //To check first time a hero can be healed
    public bool isFirstHealing;
    //To check when a player have finished the first game
    public bool isFirstGame;
    //To check when the player opens the LoadOutScreen for each time during the on boarding
    public bool isFirstGamePass;
    public bool hasOpenBefore;
    public bool isFirstLevelUp;
    public bool isShowingHeroCard;

    //Events//------------------------------------------------------------
    public delegate void OnLevelFinished(string _mapName, int _newLevel);
    public static OnLevelFinished onLevelFinished;

    public delegate void OnMapFinished(string _mapName);
    public static OnMapFinished onMapFinished;

    public delegate void OnHeroCardUnlocked(HeroCard _card = null);
    public static OnHeroCardUnlocked onHeroCardUnlocked;

    //--------------------------------------------------------------------

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        if (showSplashLogo)
        {
            if (!splashScreen.activeSelf) splashScreen.SetActive(true);
        }
        //levelsData.progress = PlayerPrefs.GetInt("progress", 0);
        isFirstTime = !PlayerPrefs.HasKey("Is_First_Time");
        isFirstSpecial = !PlayerPrefs.HasKey("Is_First_Special");
        isFirstHealing = !PlayerPrefs.HasKey("Is_First_Healing");
        isFirstGame = !PlayerPrefs.HasKey("Is_First_Game");
        isFirstLevelUp = !PlayerPrefs.HasKey("Is_First_LevelUp");
        isFirstGamePass = !PlayerPrefs.HasKey("Is_First_GamePass");
#if UNITY_EDITOR
        isFirstTime = isFirstSpecial = isFirstHealing = isFirstGame = isFirstLevelUp = isFirstGamePass = showTutorial;
        //isFirstGamePass = true;
#endif
        //isFirstTime = false;
        currencyPanelUI.gameObject.SetActive(!isFirstTime);

        allItems.ClearAllItems();

        //TO DO: Load all this info from the Firestore cloud
        int _sv = PlayerPrefs.GetInt("saveversion", 0);
        if (_sv != settings.saveVersion)
        {
            heroBase.DeleteSave();
            itemsData.DeleteSave();
            print($"Old saves deleted becouse the save version[{_sv}] doesnt match with the game version[{settings.saveVersion}]");
        }

        //heroBase.Load(_sv);
        //itemsData.Load();
        //heroBase.LoadCurrencies();

        DateTime _currentTime = DateTime.Now;
        DateTime _lastTime = DateTime.Parse(PlayerPrefs.GetString("savedtime", _currentTime.ToString("o")));
        TimeSpan _ts = _currentTime - _lastTime;

        float _ft = Mathf.Floor((float)_ts.TotalSeconds / titanTimerSet) * titanTimerSet;
        titanTimer = PlayerPrefs.GetFloat("titantimer", titanTimerSet);
        titanTimer -= titanTimerSet - ((float)_ts.TotalSeconds - _ft);
        // print($"titan timer: {titanTimer}");

        //titansData.Load((float)_ts.TotalSeconds);

        // if (_sv != settings.saveVersion)
        // {
        //     PlayerPrefs.SetInt("saveversion", settings.saveVersion);
        //     PlayerPrefs.Save();
        //     print($"Saves version set to game version [{settings.saveVersion}]");
        // }

        //SetState(GameState.GameStart);

    }

    public void StartGame()
    {
        SetState(GameState.Title);
        if (isFirstTime)
        {
            bottomTabObj.SetActive(false);
        }
        else
        {
            SetState(GameState.Title);
        }

        LeanTween.delayedCall(0.2f, () =>
        {
            FirebaseManager.instance.ActivateAuthenticator();
        });
    }

    //This function is to activate the title screen again
    public void ReturnToHome()
    {
        SetState(GameState.Title);
        //titleScreen.GetComponent<TitleManager>().InitializeTitleButtons();
    }

    private void Update()
    {
        float tmDelta = Time.deltaTime;
        if (timer > 0) { timer -= tmDelta; }

        switch (state)
        {
            case GameState.GameStart:
                if (timer <= 0)
                {
                    Debug.Log("Loading Main Menu");
                    SetState(GameState.MainMenu);
                }
                break;
        }

        if (state != GameState.Gameplay)
        {

            titansData.UpdateTimers();

            if (titanTimer > 0)
            {
                titanTimer -= tmDelta;
            }
            else
            {
                titanTimer = titanTimerSet;
                //titansData.Refresh();
            }
        }

    }

    public void SetState(GameState newstate)
    {
        notification.SetActive(newstate == GameState.Title);
        currencyPanelUI.ToggleShowHeaderText(false);
        currencyPanelUI.ToggleShowPanel(false);
        state = newstate;
        UpdateMenuButtons();
        UpdateScreenPacks();
        switch (state)
        {
            case GameState.GameStart:
                timer = .1f;
                break;
            case GameState.MainMenu:
                menuPack.SetObjects(true);
                LoadScreen(mainMenuScreen);
                //audioData.PlayMenuTrack();
                break;
            case GameState.Inventory:
                menuPack.SetObjects(true);
                LoadScreen(inventoryScreen);
                currencyPanelUI.ToggleShowPanel(true);
                universalMenuController.SetState(MenuState.Home);
                currencyPanelUI.SetText("Heroes");
                currencyPanelUI.ToggleShowHeaderText(true);
                currencyPanelUI.ToggleBackButton(true);
                break;
            case GameState.Items:
                currencyPanelUI.ToggleShowPanel(true);
                menuPack.SetObjects(true);
                universalMenuController.SetState(MenuState.Home);
                LoadScreen(itemsScreen);
                currencyPanelUI.ToggleShowPanel(true);
                currencyPanelUI.SetText("Items");
                currencyPanelUI.ToggleShowHeaderText(true);
                currencyPanelUI.ToggleBackButton(true);
                break;
            case GameState.WorldMap:
                menuPack.SetObjects(true);
                mapName.text = mapsData.GetSelectedMapName();
                LoadScreen(mapsData.GetSelectedMap());
                audioData.PlayMenuTrack();
                bottomTabObj.SetActive(true);
                currencyPanelUI.ToggleShowPanel(false);
                currencyPanelUI.ToggleShowHeaderText(false);
                currencyPanelUI.ToggleBackButton(false);
                universalMenuController.SetState(MenuState.None);
                break;
            case GameState.LevelSelect:
                LoadScreen(levelSelectScreen);
                universalMenuController.SetState(MenuState.None);
                currencyPanelUI.ToggleShowPanel(false);
                currencyPanelUI.ToggleShowHeaderText(false);
                currencyPanelUI.ToggleBackButton(false);
                break;
            case GameState.Gameplay:
                menuPack.SetObjects(false);
                LoadScreen(gameplayScreen);
                audioData.PlayBattleTrack();
                bottomTabObj.SetActive(false);
                break;
            case GameState.GameEnd:
                Application.Quit();
                break;

            case GameState.Shop:
                menuPack.SetObjects(true);
                HeroSummonScreen.SetActive(false);
                universalMenuController.SetState(MenuState.Home);
                currencyPanelUI.ToggleShowPanel(true);
                currencyPanelUI.SetText("Shop");
                currencyPanelUI.ToggleShowHeaderText(true);
                currencyPanelUI.ToggleBackButton(true);
                LoadScreen(shopScreen);
                break;
            case GameState.Settings:
                menuPack.SetObjects(true);
                universalMenuController.SetState(MenuState.Home);
                currencyPanelUI.ToggleShowPanel(true);
                currencyPanelUI.SetText("Settings");
                currencyPanelUI.ToggleShowHeaderText(false);
                currencyPanelUI.ToggleBackButton(true);
                LoadScreen(settingsScreen);
                currencyPanelUI.ToggleShowPanel(false);
                break;
            case GameState.LevelUp:
                menuPack.SetObjects(true);
                currencyPanelUI.ToggleShowPanel(true);
                LoadScreen(levelupScreen);
                break;
            case GameState.Loadout:
                menuPack.SetObjects(true);
                currencyPanelUI.ToggleShowPanel(true);
                LoadScreen(loadoutScreen);
                break;
            case GameState.HeroSelect:
                menuPack.SetObjects(true);
                universalMenuController.SetState(MenuState.Home);
                currencyPanelUI.ToggleShowPanel(true);
                LoadScreen(heroSelectScreen);
                break;
            case GameState.ItemSelect:
                menuPack.SetObjects(true);
                universalMenuController.SetState(MenuState.Home);
                currencyPanelUI.ToggleShowPanel(true);
                LoadScreen(itemSelectScreen);
                break;
            case GameState.DiamondsShop:
                HeroSummonScreen.SetActive(false);
                universalMenuController.SetState(MenuState.Home);
                menuPack.SetObjects(true);
                currencyPanelUI.ToggleShowPanel(true);
                LoadScreen(diamondsShopScreen);
                break;
            case GameState.HeroUnlock:
                menuPack.SetObjects(true);
                currencyPanelUI.ToggleShowPanel(true);
                currencyPanelUI.ToggleShowPanel(true);
                currencyPanelUI.SetText("Hero Unlock");
                currencyPanelUI.ToggleShowHeaderText(true);
                currencyPanelUI.ToggleBackButton(false);
                universalMenuController.SetState(MenuState.Home);
                LoadScreen(heroUnlockScreen);
                break;
            case GameState.MapSelect:
                menuPack.SetObjects(false);
                LoadScreen(mapSelectScreen);

                universalMenuController.SetState(MenuState.Home);
                currencyPanelUI.ToggleShowPanel(false);
                currencyPanelUI.ToggleShowHeaderText(false);
                currencyPanelUI.ToggleBackButton(false);
                break;
            case GameState.TitanSelect:
                menuPack.SetObjects(true);
                LoadScreen(titanSelectScreen);
                currencyPanelUI.ToggleShowPanel(true);
                universalMenuController.SetState(MenuState.Home);
                currencyPanelUI.SetText("Titan");
                currencyPanelUI.ToggleShowHeaderText(true);
                currencyPanelUI.ToggleBackButton(true);
                break;
            case GameState.Title:
                menuPack.SetObjects(true);
                LoadScreen(titleScreen);
                audioData.PlayMenuTrack();
                bottomTabObj.SetActive(true);

                universalMenuController.SetState(MenuState.Subscription);
                currencyPanelUI.ToggleShowPanel(true);
                currencyPanelUI.SetText("Home");
                currencyPanelUI.ToggleShowHeaderText(true);
                currencyPanelUI.ToggleBackButton(false);
                break;
        }
        kickstarterBtnController.ToggleActivateKSMessage(newstate == GameState.Title);
    }

    void LoadScreen(Manager newscreen)
    {
        if (newscreen == null) { return; }

        if (currentScreen != null)
        {
            currentScreen.Kill();
        }

        currentScreen = Instantiate(newscreen);
        currentScreen.Initialize(this);
    }

    public void GoToScreen(string _screen)
    {
        switch (_screen)
        {
            case "title": SetState(GameState.Title); break;
            case "menu": SetState(GameState.MainMenu); break;
            case "heroes": SetState(GameState.Inventory); break;
            case "map": SetState(GameState.WorldMap); break;
            case "shop": SetState(GameState.Shop); break;
            case "inventory": SetState(GameState.Items); break;
            case "settings": SetState(GameState.Settings); break;
            case "levelup": SetState(GameState.LevelUp); break;
            case "loadout": SetState(GameState.Loadout); break;
            case "exit": SetState(GameState.GameEnd); break;
            case "diamondsshop": SetState(GameState.DiamondsShop); break;
            case "mapselect": SetState(GameState.MapSelect); break;
            case "titanselect": SetState(GameState.TitanSelect); break;
        }
    }

    void UpdateMenuButtons()
    {
        int s = (int)state;
        for (int i = 0; i < menuButton.Length; i++)
        {
            if (menuButton[i] != null)
            {
                menuButton[i].SetButton(i == s);
            }
        }
    }
    void UpdateScreenPacks()
    {
        int s = (int)state;
        for (int i = 0; i < screenPacks.Length; i++)
        {
            screenPacks[i].SetObjects(i == s);
        }
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("savedtime", DateTime.Now.ToString("o"));
        PlayerPrefs.SetFloat("titantimer", titanTimer);
        titansData.SaveTitans();

        PlayerPrefs.Save();
        FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
    }

    public void AddNotification(string _text)
    {
        //notificationsManager.AddNotification(_text);
    }

    public void LoadCurrencies(float _coins, float _diamonds)
    {
        heroBase.LoadCurrencies(_coins, _diamonds);
    }
    public void LoadItemsData(string[] _itemsID, int[] _itemsAmount)
    {
        itemsData.Load(_itemsID, _itemsAmount);
    }

    public void LoadHeroesData(CloudPlayerData _cloudData)
    {
        heroBase.LoadCloudHeroes(_cloudData.heroesData);
        //heroBase.LoadLocalHeroes();
    }
    public HeroItem[] LoadDefaultHeroesData()
    {
        return heroBase.LoadDefaultHeroes();
    }

    public SaveItem[] LoadDefaultItems()
    {
        return itemsData.LoadDefaultItems();
    }

    public void OpenDiamondsIAPScreen()
    {
        SetState(GameState.Shop);
        currentScreen.GetComponent<ShopManager>().OpenShopFromTab();
    }

    public void OpenShopItemScreen()
    {
        SetState(GameState.Shop);
        currentScreen.GetComponent<ShopManager>().OpenShopFromDiamondButton();
    }

    public void OpenDailyRewardWindow()
    {
        rewardManager.ToggleDailyRewardUIScreen(true);
    }
    public void OpenDailyDealWindow()
    {
        rewardManager.ActivateDailyDealScreen();
    }
    public void OpenGoldenPassWindow()
    {
        Debug.Log("Opening golden pass");
        goldenPassManager.ToggleActivateScreen(true);
        universalMenuController.SetState(MenuState.Home);
        currencyPanelUI.ToggleShowPanel(true);
        currencyPanelUI.SetText("Battle Pass");
        currencyPanelUI.ToggleShowHeaderText(true);
        currencyPanelUI.ToggleBackButton(false);
        bottomTabObj.SetActive(false);
        kickstarterBtnController.ToggleActivateKSMessage(false);
    }

    public void OpenHeroSummonScreen()
    {
        universalMenuController.SetState(MenuState.Home);
        menuPack.SetObjects(true);
        HeroSummonScreen.SetActive(true);
        currencyPanelUI.ToggleShowPanel(true);
        currencyPanelUI.SetText("Hero Summon");
        currencyPanelUI.ToggleShowHeaderText(true);
        currencyPanelUI.ToggleBackButton(false);
    }

    public void OpenDailySpinWindow()
    {
        dailySpin.OpenDailySpinUI();
    }

    public void LoadCurrentLevel(CloudMapLevelsData _mapData)
    {
        int currentLv = 0;
        for (int i = 0; i < _mapData.worldName.Count; i++)
        {
            if (_mapData.worldName[i] == levelsData.mapName)
            {
                currentLv = _mapData.lvProgress[i];
                break;
            }
        }
        levelsData.Load(currentLv);
    }

    public Resistance[] GenerateDefaultResistances(ElementType _type)
    {
        Resistance[] resistances = new Resistance[2];
        for (int i = 0; i < colorTypeResistances.Length; i++)
        {
            if (_type == colorTypeResistances[i].colorType)
            {
                resistances = colorTypeResistances[i].colorResistances;
                break;
            }
        }
        if (resistances[0].modifier == 0) resistances[0].modifier = 1.5f;
        if (resistances[1].modifier == 0) resistances[1].modifier = 0.5f;


        return resistances;
    }

    public void ShowHeroCardAnimation(HeroCard _card)
    {
        isShowingHeroCard = true;
        heroAnimScreen.SetCard(_card, 1);
        heroAnimScreen.SetInitialValues();
        heroAnimScreen.StartScreenAnimation();
    }

    public void HideHeroCardAnimationScreen()
    {
        isShowingHeroCard = false;
        heroAnimScreen.SetInitialValues();
    }
}


public enum GameState
{
    GameStart,
    MainMenu,
    Inventory,
    Items,
    WorldMap,
    //This level select is not being used
    LevelSelect,
    Gameplay,
    GameEnd,
    Shop,
    Settings,
    LevelUp,
    Loadout,
    HeroSelect,
    ItemSelect,
    DiamondsShop,
    HeroUnlock,
    MapSelect,
    TitanSelect,
    Title
}

public enum GameplayState
{
    GameBegin,
    SpawnMobs,
    SpawnHeroes,
    SpawnGems,
    SpawnItems,
    SetTurn,
    PlayerInteract,
    CheckMatches,
    EnemyAttack,
    GameLost,
    GameWon,
    Cutscene
}

public enum ElementType
{
    None,
    Red,
    Blue,
    Green,
    Yellow,
    Purple
}

public enum Match
{
    None,
    Match3,
    Match4,
    Match5
}

public enum SwipeDirection
{
    None,
    Horizontal,
    Vertical
}

public enum ItemType
{
    None,
    Hero,
    Enemy,
    Gem,
    Global,
    Inventory,
    Consume
}

public enum Currency
{
    Coins,
    Diamonds,
    Video,
    Cash
}

public enum ItemCase
{
    Game,
    Story
}

public enum Classes
{
    Guardian,
    Ranger,
    Wizard,
    Knight,
    Archer
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum FieldSide
{
    Left,
    Mid,
    Right,
    Left3,
    Right3,
    Mid5
}

public enum RewardType
{
    HeroCard,
    Coins,
    Diamonds,
    Item
}

public enum Condition
{
    HealthGreaterThen,
    HealthLessThen,
    HealthPercentGreaterThen,
    HealthPercentLessThen,
    DeadMob
}

public enum AbilityType
{
    HealthRestore,
    MobResurrect
}

public enum CutsceneType
{
    CutsceneIntro,
    CutsceneWin,
    CutsceneLose
}

public enum AdRewardType
{
    Item,
    open_chest,
    revival_hero,
    unlock_hero,
    free_summon,
    special_item_reward,
    speen_wheel,
    shop_item_reward,
}

public enum HurtTrigger
{
    Default,
    Archer,
    Guardian,
    Knight,
    Ranger,
    Wizard
}