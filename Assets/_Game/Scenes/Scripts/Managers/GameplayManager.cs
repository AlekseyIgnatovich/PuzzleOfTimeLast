using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using TMPro;

public class GameplayManager : Manager
{

    public GameplayState state;

    [SerializeField] Transform levelFolder;
    [SerializeField] TurnPopup turnPopup;
    [SerializeField] RewardPanel rewardPanel;
    [SerializeField] NarratorPanel narratorPanel;
    [SerializeField] LevelTimer levelTimer;
    [SerializeField] TextMeshProUGUI textWaves;
    [SerializeField] GameObject adsHeroRevivalPanel;
    [SerializeField] GameObject levelFailedScreen;

    [SerializeField] OnBoardingInstruction specialInstruction;
    [SerializeField] OnBoardingInstruction healingInstruction;
    [SerializeField] GameObject gameplayInstruction;
    [SerializeField] ColorTypeResistances[] colorTypeResistances;
    public List<Gem> swapMatchGems = new List<Gem>();

    PlayerInput playerInput;


    List<string> log = new List<string>();
    List<float> logDamage = new List<float>();

    public Player player;

    public TitanData titanData;
    public Level level;
    public HeroBase heroBase;
    public MobBase mobsBase;
    public ItemsData itemsData;
    [SerializeField] GemData gemData;

    [SerializeField] Hero heroItem;
    [SerializeField] Gem gemItem;
    [SerializeField] Mob mobItem;
    [SerializeField] ItemButton itemButton;
    [Space]
    [SerializeField] SpawnPointsFolder spawnsFolder;
    [SerializeField] SpawnPoint[] spawnPoints;
    Hero[] heroes;
    Mob[] mobs;
    Gem[] gems;

    [SerializeField] Transform gemsFolder;
    [SerializeField] Transform heroesFolder;
    [SerializeField] Transform itemsFolder;
    [Space]
    [SerializeField] FloaterText floaterText;
    [SerializeField] Color missTxtColor;
    [SerializeField] TextMeshProUGUI pauseLvTMP;
    [SerializeField] TextMeshProUGUI pauseWavesTMP;
    [SerializeField] LevelsDatabase levelsDatabase;
    float timerFloats;
    [SerializeField] float timerFloatsSet = .18f;
    List<FloaterText> floaters = new List<FloaterText>();


    bool playerTurn;
    int gemType = -1;
    bool matchDone;
    float accumulatedXP;

    float timer;
    float timerCheck;
    float timerFall;
    float timerHeroCheck;
    float instructionTimer;
    [HideInInspector] public float timerMobsCheck;

    int counter;
    [HideInInspector] public int waveCount;

    [Space]
    [SerializeField] float mobSpawnDelay;
    [SerializeField] float gemSpawnDelay;
    [SerializeField] float heroSpawnDelay;

    [SerializeField] float mobAttackDelay;

    [SerializeField] float stageEndDelay;
    [Space]
    [SerializeField] AudioClip[] matchSFX;
    [SerializeField] AudioClip turnSFX;
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip loseSFX;
    [Space]
    [SerializeField] GameObject victoryEffect;

    bool specialsCharged;
    float[] specialCharge;

    Gem chosenGem;

    bool runtimer;
    float stagetimer;

    [SerializeField] bool reviveWithAds;

    bool isShowingSwap;
    bool hasStartedPlaying;
    Vector2 initScale;
    public int deathsThisLevel = 0;

    private void Awake()
    {
        reviveWithAds = true;
    }

    public override void Initialize(GameManager _manager)
    {
        log = new List<string>();
        logDamage = new List<float>();
        levelFailedScreen.SetActive(false);
        gameManager = _manager;
        canvas.worldCamera = Camera.main;
        SetState(GameplayState.GameBegin);
        gameManager.onBoardingManager.specialInstruction = specialInstruction;
        gameManager.onBoardingManager.healingInstruction = healingInstruction;
        initScale = new Vector2(0.55f, 0.55f);

        pauseLvTMP.text = "Level " + levelsDatabase.selectedLevel.ToString();
        pauseWavesTMP.text = "Wave " + (waveCount + 1).ToString() + "/" + level.waves.Length.ToString();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //LevelFailed();
            LevelWon();
        }
        if (Input.GetMouseButtonDown(0) && isShowingSwap && hasStartedPlaying)
        {
            LeanTween.cancel(swapMatchGems[0].gameObject);
            LeanTween.cancel(swapMatchGems[1].gameObject);
            //LeanTween.cancelAll();
            swapMatchGems[0].transform.localScale = initScale;
            swapMatchGems[1].transform.localScale = initScale;
            instructionTimer = 0f;
            isShowingSwap = false;
        }
        if (hasStartedPlaying && !isShowingSwap && instructionTimer < 2.5f && playerTurn)
        {
            instructionTimer += Time.deltaTime;

            if (instructionTimer >= 2.5f)
            {
                StartSwapAnimation();
            }
        }
        float tmDelta = Time.deltaTime;
        bool tmRunning = false;
        if (timer > 0) { timer -= tmDelta; tmRunning = true; }

        switch (state)
        {
            case GameplayState.GameBegin:
                if (timer <= 0)
                {
                    SetState(GameplayState.Cutscene);
                }
                break;
            case GameplayState.SpawnMobs:
                if (timer <= 0)
                {
                    timer = mobSpawnDelay;

                    counter--;

                    if (counter < 0)
                    {
                        waveCount++;
                        if (waveCount <= 1)
                        {
                            SetState(GameplayState.SpawnHeroes);
                        }
                        else
                        {
                            playerTurn = false;
                            SetState(GameplayState.SetTurn);
                        }
                    }
                    else
                    {
                        SpawnMob(counter, waveCount);
                    }
                }
                break;
            case GameplayState.SpawnHeroes:
                if (timer <= 0)
                {
                    timer = heroSpawnDelay;

                    HeroItem _hero = heroBase.GetHeroInOrder(counter);
                    heroes[counter] = Instantiate(heroItem, heroesFolder);
                    heroes[counter].SetMe(_hero, this);

                    counter--;

                    if (counter < 0)
                    {
                        SetState(GameplayState.SpawnGems);
                    }
                }
                break;
            case GameplayState.SpawnGems:
                if (timer <= 0)
                {
                    timer = gemSpawnDelay;

                    int lastgemtype = gemType;
                    int typesLength = gemData.types.Length;

                    gemType += Random.Range(1, typesLength - 1);
                    if (gemType >= typesLength) { gemType -= typesLength; }

                    Transform slot = gemsFolder.GetChild(counter);

                    gems[counter] = Instantiate(gemItem, slot.position, Quaternion.identity, slot);
                    gems[counter].transform.SetAsFirstSibling();
                    gems[counter].manager = this;
                    gems[counter].number = counter;
                    gems[counter].column = counter - (Mathf.FloorToInt(counter / 7) * 7) + 1;

                    //make sure there are no matches from start
                    if (counter < 42)
                    {
                        int gemtypebelow = (int)gems[counter + 7].Type - 1;
                        while ((gemtypebelow == gemType) || (lastgemtype == gemType))
                        {
                            gemType += Random.Range(1, typesLength - 1);
                            if (gemType >= typesLength) { gemType -= typesLength; }
                        }
                    }

                    gems[counter].SetGem(gemType);

                    counter--;

                    if (counter < 0)
                    {
                        SetState(GameplayState.SpawnItems);
                    }
                    //CheckForPossibleMatches();
                }
                break;
            case GameplayState.SpawnItems:
                ItemObject[] _items = new ItemObject[itemsData.items.Count];

                ItemButton _item;
                for (int i = 0; i < _items.Length; i++)
                {
                    _items[i] = itemsData.items[i];
                    if ((_items[i] != null) && _items[i].selected)
                    {
                        _item = Instantiate(itemButton, itemsFolder);
                        _item.Setup(i);
                    }
                }
                SetState(GameplayState.SetTurn);
                break;

            case GameplayState.SetTurn:

                if (timer <= 0)
                {
                    SetState(playerTurn ? GameplayState.PlayerInteract : GameplayState.EnemyAttack);
                }
                break;

            case GameplayState.PlayerInteract:
                break;
            case GameplayState.EnemyAttack:
                if (timerHeroCheck > 0) { break; }
                if (timer <= 0)
                {
                    //go through mobs and skip dead ones
                    for (int i = counter; i < mobs.Length; i++)
                    {
                        if (mobs[i] != null) { break; }
                        counter++;
                    }

                    if (counter < mobs.Length)
                    {
                        if (mobs[counter].counter <= 0)
                        {
                            Hero hrotrg = GetRandomHero();
                            if (hrotrg == null)
                            {
                                SetState(GameplayState.SetTurn);
                                break;
                            }
                            hrotrg.SetAsTarget(true);
                            if (mobs[counter].CheckAbilities())
                            {
                                mobs[counter].UseAbility();
                            }
                            else
                            {
                                Debug.Log("Enemies attacking");
                                mobs[counter].AttackHero(hrotrg);
                            }
                            timer = mobAttackDelay;
                            timerHeroCheck = .8f;
                        }
                        else
                        {
                            timer = .1f;
                        }
                        counter++;
                    }
                    else
                    {
                        SetState(GameplayState.SetTurn);
                    }
                }
                break;

            case GameplayState.GameLost:
                if (timer <= 0)
                {
                    if (tmRunning)
                    {
                        float _tm = Mathf.Floor(stagetimer);

                        Dictionary<string, object> parameters = new Dictionary<string, object>() {
                            { "stage", level.Name },
                            { "stage_playtime", _tm }
                        };

                        AnalyticsService.Instance.CustomData("stage_play_time", parameters);

                        if (level.titanLevel) { gameManager.titansData.RunTitanTimer(); }

                        gameManager.SetState(GameState.WorldMap);
                    }
                }
                break;
            case GameplayState.GameWon:
                if (timer <= 0)
                {
                    if (tmRunning)
                    {
                        int _tm = Mathf.FloorToInt(stagetimer);

                        Dictionary<string, object> parameters = new Dictionary<string, object>() {
                            { "stage", level.Name },
                            { "stage_playtime", _tm }
                        };

                        AnalyticsService.Instance.CustomData("stage_play_time", parameters);

                        Instantiate(rewardPanel, canvas.transform).Setup(this);

                        if (level.titanLevel) { gameManager.titansData.RunTitanTimer(); }
                        if (gameManager.isFirstGame)
                        {
                            gameManager.isFirstGame = false;
                            PlayerPrefs.SetInt("Is_First_Game", 1);
                        }
                        //gameManager.SetState(GameState.WorldMap);

                        //gameManager.SetState(GameState.MainMenu);
                    }
                }
                break;
        }

        if (timerCheck > 0)
        {
            timerCheck -= tmDelta;
            if (timerCheck <= 0)
            {
                CheckForMatches();
            }
        }
        if (timerFall > 0)
        {
            timerFall -= tmDelta;
            if (timerFall <= 0)
            {
                MoveGemsDown();
            }
        }
        if (timerHeroCheck > 0)
        {
            timerHeroCheck -= tmDelta;
            if (timerHeroCheck <= 0)
            {
                bool result = CheckIfHaveHeroes();
                Debug.Log("Checking if heroes are alive: " + result);
                if (!result)
                {
                    Debug.Log("No heroes left");
                    if (reviveWithAds)
                    {
                        level.cutsceneType = CutsceneType.CutsceneLose;
                        SetState(GameplayState.Cutscene);
                        //levelFailedScreen.SetActive(true);
                        //adsHeroRevivalPanel.SetActive(true);
                    }
                    else
                    {
                        SetState(GameplayState.GameLost);
                    }
                }
            }
        }
        if (timerMobsCheck > 0)
        {
            timerMobsCheck -= tmDelta;
            if (timerMobsCheck <= 0)
            {
                if (!CheckIfMobsExist())
                {
                    if (waveCount < level.waves.Length)
                    {
                        //DistributeExperience();
                        SetState(GameplayState.SpawnMobs);
                    }
                    else
                    {
                        level.cutsceneType = CutsceneType.CutsceneWin;
                        SetState(GameplayState.Cutscene);
                    }
                }
            }
        }
        if (specialsCharged)
        {
            specialsCharged = false;
            Hero _hero;
            float spc;
            for (int i = 0; i < heroes.Length; i++)
            {
                _hero = heroes[i];
                if ((_hero != null))
                {
                    spc = specialCharge[(int)_hero.type];
                    if (spc > 0)
                    {
                        //print($"Attempting to give {_hero.heroBase.heroCards[_hero.index].Name} {spc} special charge!");
                        _hero.GetSpecialCharge(spc);
                    }
                }
            }
            specialCharge = new float[6];
        }

        if (runtimer)
        {
            stagetimer += tmDelta;
        }

        if (timerFloats > 0)
        {
            timerFloats -= Time.deltaTime;
            if (timerFloats <= 0)
            {
                floaters[0].gameObject.SetActive(true);
                floaters.RemoveAt(0);
                if (floaters.Count > 0)
                {
                    timerFloats = timerFloatsSet;
                }
            }
        }
    }

    public void SetState(GameplayState newstate)
    {
        state = newstate;
        switch (state)
        {
            case GameplayState.GameBegin:
                timer = .5f;
                waveCount = 0;
                Level _currentlv = levelFolder.GetComponentInChildren<Level>();
                if (_currentlv != null) { Destroy(_currentlv.gameObject); }
                level = Instantiate(gameManager.levelsData.GetSelectedLevel(), levelFolder);
                playerTurn = false;
                reviveWithAds = level.reviveWithAds;

                heroes = new Hero[heroBase.GetBattleReadyHeroes()];
                gems = new Gem[gemsFolder.childCount];
                mobs = new Mob[5];
                specialCharge = new float[6];

                runtimer = true;
                stagetimer = 0f;
                break;

            case GameplayState.SpawnMobs:
                int _l = level.waves[waveCount].enemy.Length;
                spawnPoints = new SpawnPoint[_l];
                spawnPoints = spawnsFolder.GetSpawnPoints(_l);
                player.mobsTargets = new GameObject[_l];
                player.mobsTargets = spawnsFolder.GetMobTargets(_l);
                spawnsFolder.CloseHealthBars(_l);
                counter = _l;
                timer = .5f;
                //level.UpdateBackground(waveCount);
                textWaves.text = $"{waveCount + 1}/{level.waves.Length}";
                break;
            case GameplayState.SpawnHeroes:
                counter = heroes.Length - 1;
                timer = .5f;
                break;
            case GameplayState.SpawnGems:
                counter = gems.Length - 1;
                gemType = -1;
                timer = .5f;
                break;

            case GameplayState.SetTurn:
                playerTurn = !playerTurn;

                Debug.Log("Is player Turn? " + playerTurn);

                timer = .1f;
                if (playerTurn)
                {
                    HeroTurnEffects();
                    ResetMobsTurns();

                    if (hasStartedPlaying)
                    {
                        StartSwapAnimation();
                    }
                    if (gameManager.isFirstTime)
                    {
                        gameplayInstruction.SetActive(true);
                        gameManager.isFirstTime = false;
                        PlayerPrefs.SetInt("Is_First_Time", 1);
                    }
                }
                else
                {
                    MobTurnEffects();
                    DepleteMobsTurns();
                }
                //CreateTurnPopup(playerTurn ? "Your Turn" : "Enemy`s Turn", .7f);
                //gameManager.audioData.PlaySFX(turnSFX);
                break;

            case GameplayState.PlayerInteract:
                player.PlayerEnable();
                matchDone = false;
                break;
            case GameplayState.EnemyAttack:
                counter = 0;
                timer = .1f;
                break;

            case GameplayState.GameLost:
                timer = 30f; //stageEndDelay;
                levelFailedScreen.SetActive(true);
                //CreateTurnPopup("Your heroes are dead!", stageEndDelay - .5f);
                gameManager.audioData.PlaySFX(loseSFX);
                player.SetInput(false);

                if (level.titanLevel)
                {
                    UpdateTitanData();
                    //gameManager.titansData.SaveTitanHealth(mobs[0].health);
                }
                FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
                runtimer = false;

                break;
            case GameplayState.GameWon:
                timer = stageEndDelay;
                //DistributeExperience();
                heroBase.Save();
                itemsData.Save();
                CreateTurnPopup("You have defeated all enemies!", stageEndDelay - .5f);
                Instantiate(victoryEffect, new Vector3(Screen.width * .5f, Screen.height * .75f, 0), Quaternion.identity);
                gameManager.audioData.PlaySFX(winSFX);
                player.SetInput(false);
                if (level.titanLevel)
                {
                    gameManager.titansData.RefreshTitan();
                    gameManager.titansData.SaveTitans(true);
                }
                else
                {
                    gameManager.levelsData.WonLevel();
                }

                FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
                runtimer = false;
                break;

            case GameplayState.Cutscene:
                if (level.GetCutscene() == null)
                {
                    switch (level.cutsceneType)
                    {
                        case CutsceneType.CutsceneIntro:
                            SetState(GameplayState.SpawnMobs);
                            break;
                        case CutsceneType.CutsceneWin:
                            SetState(GameplayState.GameWon);
                            break;
                        case CutsceneType.CutsceneLose:
                            SetState(GameplayState.GameLost);
                            break;
                    }
                }
                else
                {
                    switch (level.cutsceneType)
                    {
                        case CutsceneType.CutsceneIntro:
                            narratorPanel.Setup(level.GetCutscene(), GameplayState.SpawnMobs);
                            break;
                        case CutsceneType.CutsceneWin:
                            narratorPanel.Setup(level.GetCutscene(), GameplayState.GameWon);
                            break;
                        case CutsceneType.CutsceneLose:
                            narratorPanel.Setup(level.GetCutscene(), GameplayState.GameLost);
                            break;
                    }
                }
                break;
        }

        void CreateTurnPopup(string _text, float _time)
        {
            Instantiate(turnPopup, canvas.transform).Init(_text, _time);
        }
    }

    private void StartSwapAnimation()
    {
        FindFirstSwapMatch();
        if (swapMatchGems.Count == 2)
        {
            isShowingSwap = true;
            LeanTween.scale(swapMatchGems[0].gameObject, initScale * 1.1f, 1f).setEaseInOutSine().setLoopPingPong();
            LeanTween.scale(swapMatchGems[1].gameObject, initScale * 1.1f, 1f).setEaseInOutSine().setLoopPingPong().setDelay(1f);
        }

    }

    #region GEMS
    public void MoveGems(Gem _gem, Vector2 _dir)
    {
        chosenGem = GetGemInGrid(_gem.number, _dir);

        if (chosenGem == null) { return; }

        SwipeDirection _swipe;
        if (Mathf.Abs(_dir.x) > Mathf.Abs(_dir.y)) { _swipe = SwipeDirection.Horizontal; }
        else { _swipe = SwipeDirection.Vertical; }

        if (chosenGem.match < Match.Match4) { chosenGem.swipe = _swipe; }
        if (_gem.match < Match.Match4) { _gem.swipe = _swipe; }

        _gem.SetMove(chosenGem);
        player.SetInput(false);
        matchDone = false;
    }

    public bool CheckForMatches()
    {
        bool matches = false;
        ElementType eType = ElementType.None;
        int match = 0;
        int matchStart = 0;
        int totalmatches = 0;
        Gem gem;
        int i = 0;
        int j = 0;
        int c = 0;

        #region Check in rolls
        //loop through grid vertically
        for (j = 0; j < 7; j++)
        {
            //loop through grid horizontally
            eType = ElementType.None;
            for (i = 0; i < 7; i++)
            {
                //get index on roll
                c = i + (j * 7);
                //get gem
                gem = gems[c];

                //if last gem type is equal to current gem type, rise match
                //ELSE reset match counter and matching starting point AFTER gems have been marked as a match IF there is a match3
                if (eType == gem.Type)
                {
                    match++;
                }
                else
                {
                    if (match > 2)
                    {
                        matches = true;
                        if (match > 3)
                        {
                            if (chosenGem != null) { chosenGem.SetChosen(match, SwipeDirection.Horizontal); }
                            else { gems[matchStart + Random.Range(0, match)].SetChosen(match, SwipeDirection.Horizontal); }
                        }
                        for (int m = 0; m < match; m++)
                        {
                            gems[matchStart + m].SetMatch(true);
                        }
                        totalmatches += match;
                    }
                    match = 1;
                    matchStart = c;
                    eType = gem.Type;
                }
            }
        }
        if (match > 2)
        {
            matches = true;
            if (match > 3)
            {
                if (chosenGem != null) { chosenGem.SetChosen(match, SwipeDirection.Horizontal); }
                else { gems[matchStart + Random.Range(0, match)].SetChosen(match, SwipeDirection.Horizontal); }
            }
            for (int m = 0; m < match; m++)
            {
                gems[matchStart + m].SetMatch(true);
            }
            totalmatches += match;
        }
        #endregion
        #region Check in columns
        //loop through grid horizontally
        for (j = 0; j < 7; j++)
        {
            //loop through grid vertically
            eType = ElementType.None;
            for (i = 0; i < 7; i++)
            {
                //get index on column
                c = j + (i * 7);
                //get gem
                gem = gems[c];

                //if last gem type is equal to current gem type, rise match
                //ELSE reset match counter and matching starting point AFTER gems have been marked as a match IF there is a match3
                if (eType == gem.Type)
                {
                    match++;
                }
                else
                {
                    if (match > 2)
                    {
                        matches = true;
                        int _gi = matchStart + Random.Range(0, match) * 7;
                        int _inx;
                        int _inxoff;
                        bool _m5d = false;//if we havent checked for a cross match 5
                        if (match > 3)
                        {
                            if (chosenGem != null) { chosenGem.SetChosen(match, SwipeDirection.Vertical); }
                            else { gems[_gi].SetChosen(match, SwipeDirection.Vertical); }
                        }
                        for (int m = 0; m < match; m++)
                        {
                            _inx = matchStart + (m * 7);
                            if (_inx < gems.Length)
                            {
                                gems[_inx].SetMatch(true);
                                if (!_m5d)
                                {
                                    _inxoff = _inx - 1;
                                    if ((_inxoff >= 0) && (gems[_inxoff].Type == eType) && (gems[_inxoff].match > 0))
                                    {
                                        _m5d = true;
                                        if (chosenGem != null) { chosenGem.SetChosen(5, SwipeDirection.Vertical); }
                                        else { gems[_gi].SetChosen(5, SwipeDirection.Vertical); }
                                    }
                                    if (!_m5d)
                                    {
                                        _inxoff = _inx + 1;
                                        if ((_inxoff < gems.Length) && (gems[_inxoff].Type == eType) && (gems[_inxoff].match > 0))
                                        {
                                            _m5d = true;
                                            if (chosenGem != null) { chosenGem.SetChosen(5, SwipeDirection.Vertical); }
                                            else { gems[_gi].SetChosen(5, SwipeDirection.Vertical); }
                                        }
                                    }
                                }
                            }
                        }
                        totalmatches += match;
                    }
                    match = 1;
                    matchStart = c;
                    eType = gem.Type;
                }
            }
        }
        if (match > 2)
        {
            matches = true;
            int _gi = matchStart + Random.Range(0, match) * 7;
            int _inx;
            int _inxoff;
            bool _m5d = false;//if we havent checked for a cross match 5
            if (match > 3)
            {
                if (chosenGem != null) { chosenGem.SetChosen(match, SwipeDirection.Vertical); }
                else { gems[_gi].SetChosen(match, SwipeDirection.Vertical); }
            }
            for (int m = 0; m < match; m++)
            {
                _inx = matchStart + (m * 7);
                if (_inx < gems.Length)
                {
                    gems[_inx].SetMatch(true);
                    if (!_m5d)
                    {
                        _inxoff = _inx - 1;
                        if ((_inxoff >= 0) && (gems[_inxoff].Type == eType) && (gems[_inxoff].match > 0))
                        {
                            _m5d = true;
                            if (chosenGem != null) { chosenGem.SetChosen(5, SwipeDirection.Vertical); }
                            else { gems[_gi].SetChosen(5, SwipeDirection.Vertical); }
                        }
                        if (!_m5d)
                        {
                            _inxoff = _inx + 1;
                            if ((_inxoff < gems.Length) && (gems[_inxoff].Type == eType) && (gems[_inxoff].match > 0))
                            {
                                _m5d = true;
                                if (chosenGem != null) { chosenGem.SetChosen(5, SwipeDirection.Vertical); }
                                else { gems[_gi].SetChosen(5, SwipeDirection.Vertical); }
                            }
                        }
                    }
                }
            }
            totalmatches += match;
        }
        #endregion

        chosenGem = null;
        if (matches)
        {
            timerFall = .5f;
            matchDone = true;

            int matchsfx = totalmatches > 3 ? (totalmatches > 4 ? 2 : 1) : 0;
            gameManager.audioData.PlaySFX(matchSFX[matchsfx]);
            return true;
        }
        else
        {
            if (matchDone)
            {
                if (CheckIfMobsExist())
                {
                    SetState(GameplayState.SetTurn);
                }
                else
                {
                    if (waveCount < level.waves.Length)
                    {
                        //DistributeExperience();
                        SetState(GameplayState.SpawnMobs);
                    }
                    else
                    {
                        level.cutsceneType = CutsceneType.CutsceneWin;
                        SetState(GameplayState.Cutscene);
                    }
                }
            }
            else
            {
                player.SetInput(true);
            }
            return false;
        }
    }

    public Gem GetGemInGrid(int origin, Vector2 dir)
    {
        int x = 0;
        int y = 0;
        int num = origin;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            x = (int)Mathf.Sign(dir.x);
        }
        else
        {
            y = -(int)Mathf.Sign(dir.y);
        }

        num += y * 7;

        if ((num < 0) || (num >= gems.Length)) { return null; }

        int xoffset = (num - (Mathf.FloorToInt(num / 7) * 7)) + x;
        if ((xoffset < 0) || (xoffset > 6)) { return null; }

        num += x;

        return gems[num];
    }

    void MoveGemsDown()
    {
        //print("shifting");
        bool shifting = true;
        Gem _gemb;
        Gem _gem;
        Match mc;
        while (shifting)
        {
            shifting = false;
            for (int i = gems.Length - 1; i >= 0; i--)
            {
                _gemb = GetGemInGrid(i, Vector2.down);
                if (_gemb != null)
                {
                    _gem = gems[i];
                    if (!_gem.dead && _gemb.dead)
                    {
                        _gem.dead = true;
                        _gemb.dead = false;
                        _gemb.SetGem((int)_gem.Type - 1);
                        _gemb.shiftCount++;
                        mc = _gemb.match;
                        _gemb.match = _gem.match;
                        _gemb.swipe = _gem.swipe;
                        _gem.match = mc;
                        _gemb.UpdateGraphics();
                        _gem.UpdateGraphics();
                        shifting = true;
                    }
                }
                //print($"shift check");
            }
        }

        for (int i = 0; i < gems.Length; i++)
        {
            if (gems[i].dead)
            {
                gems[i].ResetGem();
            }
            else
            {
                gems[i].animator.Play("idle", 0, 0);
            }
            gems[i].ShiftCheck();
        }

        timerCheck = 1f;
    }

    public void DamageColumn(int _column, ElementType _type)
    {
        Mob _mob;
        float _atk;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            _mob = spawnPoints[i].MyMob;
            if (_mob != null)
            {
                if (_mob.OnColumn(_column))
                {
                    _atk = GetHeroAttack(_type);
                    _mob.StoreAttacks(_atk, _type, _atk > 0, HurtTrigger.Default);

                    if (level.titanLevel)
                    {
                        for (int j = 0; j < heroes.Length; j++)
                        {
                            if ((heroes[j] != null) && (heroes[j].type == _type))
                            {
                                log.Add($"{heroBase.username} attacked for {heroes[j].GetAttack()} Damage!");
                                logDamage.Add(heroes[j].GetAttack());
                                //titanData.GetTitanSet().LogAttack($"{heroBase.username} attacked for {heroes[j].GetAttack()} Damage!", heroes[j].GetAttack());
                            }
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < spawnPoints[i].columns.Length; j++)
                {
                    if (spawnPoints[i].columns[j] == _column)
                    {
                        FloaterText _ftx = Instantiate(floaterText, spawnPoints[i].manager.canvas.transform);
                        _ftx.transform.position = spawnPoints[i].transform.position + (Random.value * .23f * new Vector3(1 * (Random.value > .5f ? 1 : -1), 1 * (Random.value > .5f ? 1 : -1), 0));
                        _ftx.Setup("MISS", 80, missTxtColor);
                        _ftx.gameObject.SetActive(false);

                        AddFloater(_ftx);
                        break;
                    }
                }
            }

            if (!CheckIfHaveHeroElement(_type))
            {
                if (spawnPoints[i].HasColumn(_column))
                {
                    spawnPoints[i].SetNoHero();
                }
            }
        }
    }

    public void Match4RollExecute(int _num)
    {
        print("MATCH WHOLE ROLL");
        int _startpoint = (int)Mathf.Floor((float)_num / 7) * 7;
        for (int i = _startpoint; i < (_startpoint + 7); i++)
        {
            gems[i].SetMatch(true);
        }
        timerFall = .5f;
        matchDone = true;
        gameManager.audioData.PlaySFX(matchSFX[2]);
    }
    public void Match4ColumnExecute(int _col)
    {
        print("MATCH WHOLE COLUMN");
        _col--;
        for (int i = 0; i < 7; i++)
        {
            gems[_col + (i * 7)].SetMatch(true);
        }
        timerFall = .5f;
        matchDone = true;
        gameManager.audioData.PlaySFX(matchSFX[2]);
    }

    public void Match5Execute(ElementType _type)
    {
        for (int i = 0; i < gems.Length; i++)
        {
            if (gems[i] != null)
            {
                if (gems[i].Type == _type)
                {
                    gems[i].SetMatch(true);
                }
            }
        }
        timerFall = .5f;
        matchDone = true;
        gameManager.audioData.PlaySFX(matchSFX[2]);
    }

    public void ShuffleAllGems()
    {
        print("All Gems got shuffled");
        ElementType _type;
        Match _match;
        SwipeDirection _swipe;

        Gem _gem1;
        Gem _gem2;

        for (int i = 0; i < gems.Length; i++)
        {
            _gem1 = gems[i];
            _gem2 = gems[Random.Range(0, gems.Length)];

            _type = gems[i].Type;
            _match = gems[i].match;
            _swipe = gems[i].swipe;

            _gem1.Type = _gem2.Type;
            _gem1.match = _gem2.match;
            _gem1.swipe = _gem2.swipe;

            _gem2.Type = _type;
            _gem2.match = _match;
            _gem2.swipe = _swipe;
        }

        for (int i = 0; i < gems.Length; i++)
        {
            gems[i].UpdateGraphics();
        }

        timerCheck = .05f;
    }

    public void StartGemsSwapSelecting(Gem _gem1)
    {
        _gem1.SelectGem();
        player.StartSelectingGems(_gem1);
    }
    public void ShiftSelectedGems(Gem _gem1, Gem _gem2)
    {
        ElementType _type;
        Match _match;
        SwipeDirection _swipe;

        _type = _gem1.Type;
        _match = _gem1.match;
        _swipe = _gem1.swipe;

        _gem1.Type = _gem2.Type;
        _gem1.match = _gem2.match;
        _gem1.swipe = _gem2.swipe;

        _gem2.Type = _type;
        _gem2.match = _match;
        _gem2.swipe = _swipe;

        _gem1.UpdateGraphics();
        _gem2.UpdateGraphics();

        timerCheck = .05f;
    }

    #endregion
    #region MOBS
    public void SpawnMob(int _index, int _wave)
    {
        MobCard _mob = mobsBase.mob[0];

        //print($"index [{_index}] wave [{_wave}]");
        //print($"waves [{level.waves.Length}]");
        //print($"enemies [{level.waves[_wave].enemy.Length}]");

        _mob = level.titanLevel ? gameManager.titansData.GetTitan() : level.waves[_wave].enemy[_index].mob;
        if (level.titanLevel) gameManager.titansData.once = false;

        EnemyPoint _enm = level.waves[_wave].enemy[_index];
        //spawnPoints[_index].type = _enm.type;
        //spawnPoints[_index].type = _mob.isCustom ? _mob.type : (ElementType)level.GenerateValueForLevel(level.Name, 5);
        if (level.titanLevel) _enm.mob = _mob;
        spawnPoints[_index].type = _enm.mob.type;

        if (_enm.mob.isCustom)
        {
            spawnPoints[_index].resistance = new Resistance[_enm.mob.resistances.Length];
            for (int i = 0; i < spawnPoints[_index].resistance.Length; i++)
            {
                if (i < _enm.resistance.Length)
                {
                    spawnPoints[_index].resistance[i] = new Resistance();
                    spawnPoints[_index].resistance[i].type = _enm.mob.resistances[i].type;
                    //spawnPoints[_index].resistance[i].type = _mob.resistances[i].type;
                    spawnPoints[_index].resistance[i].modifier = _enm.mob.resistances[i].modifier;
                    //spawnPoints[_index].resistance[i].modifier = _mob.resistances[i].modifier;
                    spawnPoints[_index].UpdateColor();
                }
            }
        }
        else
        {
            spawnPoints[_index].resistance = gameManager.GenerateDefaultResistances(spawnPoints[_index].type);
            spawnPoints[_index].UpdateColor();
        }

        if (_mob != null)
        {
            mobs[_index] = Instantiate(mobItem, spawnPoints[_index].transform.position, Quaternion.identity, spawnPoints[_index].transform);
            mobs[_index].SetMob(spawnPoints[_index], _mob);

            spawnPoints[_index].MyMob = mobs[_index];

            if (level.titanLevel)
            {
                mobs[_index].maxHealth = gameManager.titansData.GetTitanMaxHealth();
                mobs[_index].health = gameManager.titansData.GetTitanHealth();
            }
        }
    }
    void DepleteMobsTurns()
    {
        for (int i = 0; i < mobs.Length; i++)
        {
            if (mobs[i] != null)
            {
                mobs[i].DepleteTurnCounter();
            }
        }
    }
    void ResetMobsTurns()
    {
        for (int i = 0; i < mobs.Length; i++)
        {
            if (mobs[i] != null)
            {
                mobs[i].ResetTurnCounter();
            }
        }
    }
    bool CheckIfMobsExist()
    {
        for (int i = 0; i < mobs.Length; i++)
        {
            if (mobs[i] != null) { return true; }
        }
        return false;
    }
    public Mob[] GetMobs()
    {
        return mobs;
    }
    void MobTurnEffects()
    {
        for (int i = 0; i < mobs.Length; i++)
        {
            if (mobs[i] != null)
            {
                mobs[i].OnTurnEffects();
            }
        }
    }
    public void UpdateSelectedTarget(Mob _mob)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].UpdateTargetReticle(spawnPoints[i].MyMob == _mob);
        }
    }
    public Mob GetRandomMob()
    {
        Mob _mob = null;
        bool _hasmob = false;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].MyMob != null)
            {
                _hasmob = true;
                break;
            }
        }
        if (_hasmob)
        {
            while (_mob == null)
            {
                _mob = spawnPoints[Random.Range(0, spawnPoints.Length)].MyMob;
            }
        }
        return _mob;
    }
    #endregion
    #region HEROES
    float GetHeroAttack(ElementType _type)
    {
        float _attack = 0;
        for (int i = 0; i < heroes.Length; i++)
        {
            if ((heroes[i] != null) && (heroes[i].type == _type))
            {
                _attack += heroes[i].GetAttack();
                heroes[i].AttackEnemy();
            }
        }
        return _attack;
    }
    Hero GetRandomHero()
    {
        if (!CheckIfHaveHeroes()) { return null; }

        Hero _hero = heroes[Random.Range(0, heroes.Length)];
        while (_hero == null)
        {
            _hero = heroes[Random.Range(0, heroes.Length)];
        }
        return _hero;
    }
    bool CheckIfHaveHeroes()
    {
        Debug.Log("Checking if have heroes");
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null) { return true; }
        }
        return false;
    }
    public void AddHeroSpecial(ElementType _type)
    {
        specialsCharged = true;
        specialCharge[(int)_type]++;
    }
    public void AccumulateXP(float amount)
    {
        accumulatedXP += amount;
    }
    void DistributeExperience()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null)
            {
                heroes[i].GetExperience(accumulatedXP);
                heroBase.hero[i].SetStats(heroes[i]);
            }
        }
    }
    public Hero[] GetHeros()
    {
        return heroes;
    }
    void HeroTurnEffects()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] != null)
            {
                heroes[i].OnTurnEffects();
            }
        }
    }

    bool CheckIfHaveHeroElement(ElementType _element)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            if ((heroes[i] != null) && (heroes[i].type == _element)) { return true; }
        }
        return false;
    }

    public void AdsHeroRevival()
    {

        heroes = new Hero[heroBase.GetBattleReadyHeroes()];

        int i = 0;
        HeroItem _hero;
        while (i < heroes.Length)
        {
            _hero = heroBase.GetHeroInOrder(i);
            heroes[i] = Instantiate(heroItem, heroesFolder);
            heroes[i].SetMe(_hero, this);
            heroes[i].health *= .5f;
            heroes[i].specialPoints = 0;
            heroes[i].healthBar.UpdateHealth(heroes[i].health, heroes[i].maxHealth, false);
            heroes[i].specialBar.UpdateHealth(heroes[i].specialPoints, heroes[i].specialMaxPoints, false);
            i++;
        }

        SetState(GameplayState.PlayerInteract);
    }

    public void GemsHeroRevival()
    {
        heroes = new Hero[heroBase.GetBattleReadyHeroes()];

        int i = 0;
        HeroItem _hero;
        while (i < heroes.Length)
        {
            _hero = heroBase.GetHeroInOrder(i);
            heroes[i] = Instantiate(heroItem, heroesFolder);
            heroes[i].SetMe(_hero, this);
            //heroes[i].health *= .5f;
            heroes[i].specialPoints = 0;
            heroes[i].healthBar.UpdateHealth(heroes[i].health, heroes[i].maxHealth, false);
            heroes[i].specialBar.UpdateHealth(heroes[i].specialPoints, heroes[i].specialMaxPoints, false);
            i++;
        }

        SetState(GameplayState.PlayerInteract);
    }
    #endregion

    public void GaveUpBtn()
    {
        if (level.titanLevel)
        {
            UpdateTitanData();
        }
        FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
        gameManager.SetState(GameState.WorldMap);
    }

    public void WinTestBtn()
    {
        LevelWon();
    }

    public void GoToWorldMap()
    {
        FirebaseManager.instance.firestoreManager.UpdatePlayerDatabase();
        gameManager.SetState(GameState.WorldMap);
    }

    void UpdateTitanData()
    {
        //gameManager.titansData.SaveTitanHealth(mobs[0].health);
        gameManager.titansData.RunTitanTimer();
        string _log = "";
        float _dmg = 0;
        for (int i = 0; i < log.Count; i++)
        {
            _log += log[i] + "\n";
            _dmg += logDamage[i];
        }
        gameManager.titansData.GetTitanSet().LogAttack(_log, _dmg);
        gameManager.titansData.SaveTitans();
    }
    public void CloseAnyPopup()
    {

    }

    public void PauseGame(bool _isPausing)
    {
        Time.timeScale = _isPausing ? 0 : 1;
    }

    public void RestartLevel()
    {
        gameManager.SetState(GameState.Gameplay);
    }

    public void ReviveHeroesWithAds()
    {
        //gameManager.ads.SetupForReward(AdRewardType.HeroRevival, null);
        AdsHeroRevival();
        print("Revived heroes with ads.");
        //videoCanvas.OpenVideo(shopItem);
    }

    public void ReviveWithGems()
    {
        if (FirebaseManager.instance.firestoreManager.ModifyDiamondsAmount(-10))
        {
            GemsHeroRevival();
        }
        else
        {
            Debug.Log("Player doesn't have enough diamonds");
        }

    }

    public void LevelFailed()
    {
        SetState(GameplayState.GameLost);
    }

    public void LevelWon()
    {
        SetState(GameplayState.GameWon);
    }

    public void AddFloater(FloaterText _floater)
    {
        floaters.Add(_floater);
        if (timerFloats <= 0) { timerFloats = timerFloatsSet; }
    }

    public void FindFirstSwapMatch()
    {
        swapMatchGems.Clear();

        int width = 7;
        int height = 6;

        // Recorrer el tablero
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                // Intentar intercambiar con la gema a la derecha (si existe)
                if (col < width - 1 && CanSwapAndMatch(row, col, row, col + 1))
                {
                    // Guardar las dos gemas intercambiadas y detener la búsqueda
                    swapMatchGems.Add(gems[row * 7 + col]);
                    swapMatchGems.Add(gems[row * 7 + (col + 1)]);
                    Debug.Log("Match found at: (" + row + ", " + col + ") and (" + row + ", " + (col + 1) + ")");
                    return;
                }

                // Intentar intercambiar con la gema debajo (si existe)
                if (row < height - 1 && CanSwapAndMatch(row, col, row + 1, col))
                {
                    // Guardar las dos gemas intercambiadas y detener la búsqueda
                    swapMatchGems.Add(gems[row * 7 + col]);
                    swapMatchGems.Add(gems[(row + 1) * 7 + col]);
                    Debug.Log("Match found at: (" + row + ", " + col + ") and (" + (row + 1) + ", " + col + ")");
                    return;
                }
            }
        }

        Debug.Log("No match found.");
    }

    // Función para verificar si el intercambio de dos gemas genera un match
    private bool CanSwapAndMatch(int row1, int col1, int row2, int col2)
    {
        // Intercambiar temporalmente las gemas
        Gem temp = gems[row1 * 7 + col1];
        gems[row1 * 7 + col1] = gems[row2 * 7 + col2];
        gems[row2 * 7 + col2] = temp;

        // Revisar si el intercambio genera un match
        bool isMatch = CheckMatchAt(row1, col1) || CheckMatchAt(row2, col2);

        // Deshacer el intercambio
        temp = gems[row1 * 7 + col1];
        gems[row1 * 7 + col1] = gems[row2 * 7 + col2];
        gems[row2 * 7 + col2] = temp;

        return isMatch;
    }

    // Función para verificar si hay un match en una posición específica
    private bool CheckMatchAt(int row, int col)
    {
        ElementType gemType = gems[row * 7 + col].Type;

        // Verificar match horizontal
        int horizontalCount = 1;
        for (int i = col - 1; i >= 0 && gems[row * 7 + i].Type == gemType; i--)
        {
            horizontalCount++;
        }
        for (int i = col + 1; i < 7 && gems[row * 7 + i].Type == gemType; i++)
        {
            horizontalCount++;
        }
        if (horizontalCount >= 3)
        {
            Debug.Log("Horizontal match at: (" + row + ", " + col + ")");
            return true;
        }

        // Verificar match vertical
        int verticalCount = 1;
        for (int i = row - 1; i >= 0 && gems[i * 7 + col].Type == gemType; i--)
        {
            verticalCount++;
        }
        for (int i = row + 1; i < 6 && gems[i * 7 + col].Type == gemType; i++)
        {
            verticalCount++;
        }
        if (verticalCount >= 3)
        {
            Debug.Log("Vertical match at: (" + row + ", " + col + ")");
            return true;
        }

        return false;
    }

    public void StartPlaying()
    {
        hasStartedPlaying = true;
    }

    public void CalculateAverageCardsLost()
    {
        int levelsPlayed = FirebaseManager.instance.firestoreManager.levelsCompletedInSession;
        float averageDeathsPerLevel = (float)deathsThisLevel / levelsPlayed;

        // Enviar el evento de promedio de muertes de cartas por nivel
        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            { "average_card_deaths_per_level", averageDeathsPerLevel }
        };
        AnalyticsService.Instance.CustomData("average_card_deaths_per_level", parameters);

        // Reiniciar el contador para el siguiente nivel
        deathsThisLevel = 0;
    }

    public void RegisterHeroDeath()
    {
        Debug.Log("Hero death registered!");
        deathsThisLevel++;
    }
}

[System.Serializable]
public class ColorTypeResistances
{
    public ElementType colorType;
    public Resistance[] colorResistances;
}