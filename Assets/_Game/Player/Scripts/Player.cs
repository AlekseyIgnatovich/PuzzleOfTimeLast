using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    [SerializeField] ItemsData itemData;
    public GameplayManager manager;
    [SerializeField] GraphicRaycaster raycaster;
    [SerializeField] Transform specialCursor;
    [SerializeField] InfoPopup infoPop;
    [SerializeField] GemPopup gemPop;
    [SerializeField] Transform allHeroesFolder;
    [SerializeField] Transform heroesFolder;
    [SerializeField] Transform selectedFolder;

    InfoPopup popInfo;
    GemPopup popGem;

    PlayerInput input;

    Hero heroSelected;
    bool specialTargeting;
    bool cardCheck;
    bool inputting;
    Vector2 touchPosition;
    [SerializeField] float moveDistance;
    [Space]
    [SerializeField] AudioClip selectActorSFX;

    Hero lastHeroSelected;

    EventSystem eventSystem;

    float timer;
    [SerializeField] float tapTime;
    [Space]
    [SerializeField] HeroHighlight heroHighlight;
    [SerializeField] Transform itemHighlight;
    [SerializeField] Transform itemsFolder;
    [SerializeField] Transform abilitiesFolder;
    [SerializeField] AbilityButton abilityButton;
    [Space]
    [SerializeField] Hero selectedHero;
    [SerializeField] Mob selectedMob;
    [SerializeField] ItemButton selectedItem;
    [SerializeField] AbilityButton selectedAbility;
    [SerializeField] Gem selectedGem;
    [Space]
    [SerializeField] Hero heroInUse;
    [SerializeField] ItemButton itemInUse;
    [SerializeField] AbilityButton abilityInUse;
    [Space]
    [SerializeField] HeroInfoPanel heroInfoPanel;
    [SerializeField] MobInfoPanel mobInfoPanel;
    [SerializeField] ItemInfoPanel itemInfoPanel;
    [SerializeField] AbilityInfoPanel abilityInfoPanel;
    [Space]
    [SerializeField] Mob targetedMob;
    [SerializeField] GameObject heroesTarget;
    [SerializeField] GameObject gemsTarget;
    public GameObject[] mobsTargets;

    public bool selectingGems;
    Gem gemSelect1;
    Gem gemSelect2;

    public Mob TargetedMob
    {
        get { return targetedMob; }
        set
        {
            targetedMob = value;
            manager.UpdateSelectedTarget(targetedMob);
        }
    }

    private void Awake()
    {
        input = new PlayerInput();
    }
    private void OnDisable()
    {
        SetInput(false);
    }

    private void Start()
    {
        input.Player.Touch.started += OnPress;
        input.Player.Touch.canceled += OnPress;

        eventSystem = FindObjectOfType<EventSystem>();
        SetInput(false);
        CloseInfoPanels();
        ClearTargets();

        selectingGems = false;
    }
    private void Update()
    {
        Vector2 pos = input.Player.Move.ReadValue<Vector2>();
        if (inputting)
        {
            if (selectedGem != null)
            {
                if (Vector2.Distance(touchPosition, pos) > moveDistance)
                {
                    inputting = false;
                    manager.MoveGems(selectedGem, (pos - touchPosition).normalized);
                    selectedGem = null;
                }
            }
        }
        if (specialTargeting)
        {
            specialCursor.position = pos;
        }
        if (lastHeroSelected != heroSelected)
        {
            lastHeroSelected = heroSelected;
            HeroSelectionChange();
            if (heroSelected == null)
            {
                if (popInfo != null) { popInfo.Kill(); }
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                OnHoldExecute();
            }
        }
    }
    public void PlayerEnable()
    {
        Debug.Log("Activando Input");
        SetInput(true);
    }
    public void PlayerDisable()
    {
        SetInput(false);
    }

    void OnPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            timer = tapTime;

            Vector2 uipos = input.Player.Move.ReadValue<Vector2>();
            PointerEventData pointED = new PointerEventData(eventSystem) { position = uipos };
            List<RaycastResult> _result = new List<RaycastResult>();
            raycaster.Raycast(pointED, _result);

            bool _epp = true;

            //IF WE HAVE RESAULTS
            if (_result.Count > 0)
            {
                _epp = false;
                //TargetedMob = null;
                EndSelectingGems();
                for (int i = 0; i < _result.Count; i++)
                {

                    //IF RESAULT IS AN ITEM FOR USE
                    selectedItem = _result[i].gameObject.GetComponent<ItemButton>();
                    if (selectedItem != null)
                    {
                        return;
                    }
                    //IF RESAULT IS AN ABILITY
                    selectedAbility = _result[i].gameObject.GetComponent<AbilityButton>();
                    if (selectedAbility != null) { return; }
                }
            }
            else
            {
                inputting = true;
                //CHECK FOR HIT IN REAL WORLD
                touchPosition = input.Player.Move.ReadValue<Vector2>();
                Vector2 pos = Camera.main.ScreenToWorldPoint(touchPosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward);

                if (hit)
                {
                    _epp = false;
                    //IF HIT A RAY ABSORBER
                    RayAbsorber _ra = hit.collider.GetComponent<RayAbsorber>();
                    if (_ra != null)
                    {
                        EndSelectingGems();
                        return;
                    }
                    //IF HIT IS A MOB
                    Mob _mob = hit.collider.GetComponent<Mob>();
                    if (_mob != null)
                    {
                        selectedMob = _mob;
                        TargetedMob = _mob;
                        return;
                    }
                    //IF HIT IS A GEM
                    selectedGem = hit.collider.GetComponent<Gem>();
                    if (selectedGem != null)
                    {
                        TargetedMob = null;
                        selectedGem.animator.SetTrigger("press");
                        if (selectingGems)
                        {
                            if (gemSelect1 == null)
                            {
                                gemSelect1 = selectedGem;
                            }
                            else
                            {
                                gemSelect2 = selectedGem;
                                selectingGems = false;
                                manager.ShiftSelectedGems(gemSelect1, gemSelect2);
                            }
                            selectedGem = null;
                        }
                        return;
                    }
                }
                EndSelectingGems();
            }

            inputting = true;

            //CHECK IF WE HIT HERO
            if (_result.Count > 0)
            {
                _epp = false;
                Debug.Log("Selecting Hero");
                EndSelectingGems();
                Hero _hero;
                for (int i = 0; i < _result.Count; i++)
                {
                    _hero = _result[i].gameObject.GetComponent<Hero>();
                    if (_hero != null)
                    {
                        selectedHero = _hero;
                        break;
                    }
                }
            }

            if (_epp) { TargetedMob = null; }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (context.canceled)
        {

            if (timer > 0)
            {
                TapInput();
            }
            else
            {
                ReleaseInput();
            }
            timer = 0;
            selectedHero = null;
            selectedMob = null;
            selectedItem = null;
            selectedAbility = null;
            selectedGem = null;

            inputting = false;
            specialTargeting = false;
            specialCursor.gameObject.SetActive(specialTargeting);
        }
    }

    void TapInput()
    {
        //print("Tapped");
        bool _got = false;

        heroInUse = selectedHero;
        if ((itemInUse != null) || (heroInUse != null)/*(abilityInUse != null)*/)
        {
            if (/*abilityInUse*/(heroInUse != null) && (heroInUse.specialPoints >= heroInUse.specialMaxPoints) && itemInUse == null)
            {
                if (targetedMob == null)
                {
                    //Debug.Log("No tiene target");
                    TargetedMob = manager.GetRandomMob();
                }
                heroInUse.SpecialAttackEnemy(selectedHero, targetedMob);
                manager.timerMobsCheck = .5f;
            }
            if (itemInUse != null)
            {
                Item _item = itemData.items[itemInUse.index].item;
                if (_item.effect.ApplyEffect(manager, selectedHero, selectedMob, selectedGem))
                {
                    print($"used {_item.Name} item");
                    if (!itemData.items[itemInUse.index].SubtractAmount())
                    {
                        itemData.items[itemInUse.index].selected = false;
                        itemData.items[itemInUse.index] = null;
                        Destroy(itemInUse.gameObject);
                    }
                }
            }

            itemInUse = null;
            abilityInUse = null;
            heroInUse = null;
            ClearTargets();
        }
        else
        {
            if (selectedHero != null)
            {
                _got = true;
                heroInUse = selectedHero;
                if (heroInUse.specialPoints >= heroInUse.specialMaxPoints)
                {
                    SetTargets(null, heroInUse.card.heroClass.special);
                    //heroHighlight.Setup(heroInUse.type, heroInUse.transform.position);
                    //abilityInUse = selectedAbility;
                    //itemHighlight.gameObject.SetActive(true);
                    //itemHighlight.position = abilityInUse.transform.position;
                    //SetTargets(null, abilityInUse.GetSpecial());
                }
                //OpenAbilities(selectedHero);
            }
            if (selectedMob != null)
            {
                _got = true;
            }
            if (selectedItem != null)
            {
                itemInUse = selectedItem;
                itemHighlight.gameObject.SetActive(true);
                itemHighlight.position = itemInUse.transform.position;
                SetTargets(itemData.items[itemInUse.index].item, null);
                _got = true;
            }
            if (selectedAbility != null)
            {
                if (heroInUse.specialPoints >= heroInUse.specialMaxPoints)
                {
                    abilityInUse = selectedAbility;
                    itemHighlight.gameObject.SetActive(true);
                    itemHighlight.position = abilityInUse.transform.position;
                    SetTargets(null, abilityInUse.GetSpecial());
                }
                _got = true;
            }
            if (selectedGem != null)
            {
                _got = true;
                ExecuteChargedGem(selectedGem);
            }
        }

        //if nothing is selected
        if (!_got)
        {
            abilitiesFolder.gameObject.SetActive(false);
            heroHighlight.gameObject.SetActive(false);
            itemHighlight.gameObject.SetActive(false);
            itemsFolder.gameObject.SetActive(true);
        }
    }
    void ReleaseInput()
    {
        print("Released");
        CloseInfoPanels();
    }
    void OnHoldExecute()
    {
        print("Holding Down");
        bool _panel = false;
        if (selectedHero != null)
        {
            _panel = true;
            heroInfoPanel.Setup(selectedHero);
        }
        if (selectedMob != null)
        {
            _panel = true;
            mobInfoPanel.Setup(selectedMob);
        }
        if (selectedItem != null)
        {
            _panel = true;
            itemInfoPanel.Setup(itemData.items[selectedItem.index]);
        }
        if (selectedAbility != null)
        {
            _panel = true;
            abilityInfoPanel.Setup(selectedAbility);
        }
        if (selectedGem != null)
        {
            ExecuteChargedGem(selectedGem);
        }
        if (_panel)
        {
            manager.gameManager.audioData.PlaySFX(selectActorSFX);
        }
    }

    public void StartSelectingGems(Gem _gem1)
    {
        selectingGems = true;
        selectedGem = null;
        gemSelect1 = _gem1;
        gemSelect2 = null;
    }
    public void EndSelectingGems()
    {
        selectingGems = false;
        selectedGem = null;
    }

    void ExecuteChargedGem(Gem _gem)
    {
        //print("trying to execute gem charge");
        if (_gem.match == Match.Match4)
        {
            _gem.match = Match.None;
            if (_gem.swipe == SwipeDirection.Horizontal)
            {
                manager.Match4RollExecute(_gem.number);
            }
            if (_gem.swipe == SwipeDirection.Vertical)
            {
                manager.Match4ColumnExecute(_gem.column);
            }
        }
        if (_gem.match == Match.Match5)
        {
            _gem.match = Match.None;
            manager.Match5Execute(_gem.Type);
        }
    }

    void OpenAbilities(Hero _hero)
    {
        abilitiesFolder.gameObject.SetActive(true);
        itemsFolder.gameObject.SetActive(false);

        if (abilitiesFolder.childCount > 0)
        {
            Transform[] _chlds = abilitiesFolder.GetComponentsInChildren<Transform>(true);
            for (int i = 1; i < _chlds.Length; i++)
            {
                Destroy(_chlds[i].gameObject);
            }
        }

        Instantiate(abilityButton, abilitiesFolder).Setup(_hero);

        heroHighlight.Setup(_hero.type, _hero.transform.position);
    }

    void CloseInfoPanels()
    {
        heroInfoPanel.gameObject.SetActive(false);
        mobInfoPanel.gameObject.SetActive(false);
        itemInfoPanel.gameObject.SetActive(false);
        abilityInfoPanel.gameObject.SetActive(false);
    }

    void SetTargets(Item _item, HeroSpecial _ability)
    {
        ItemType _target = ItemType.None;

        if (_item != null) { _target = _item.targetType; }
        if (_ability != null) { _target = _ability.targetType; }

        Mob[] _mobs = manager.GetMobs();
        switch (_target)
        {
            case ItemType.Hero:
                heroesTarget.SetActive(true);
                break;
            case ItemType.Enemy:
                for (int i = 0; i < mobsTargets.Length; i++)
                {
                    mobsTargets[i].SetActive(_mobs[i] != null);
                }
                break;
            case ItemType.Gem:
                gemsTarget.SetActive(true);
                break;

            case ItemType.Global:
                heroesTarget.SetActive(true);
                for (int i = 0; i < mobsTargets.Length; i++)
                {
                    mobsTargets[i].SetActive(_mobs[i] != null);
                }
                break;
        }
    }
    void ClearTargets()
    {
        heroesTarget.SetActive(false);
        gemsTarget.SetActive(false);
        for (int i = 0; i < mobsTargets.Length; i++)
        {
            mobsTargets[i].SetActive(false);
        }
    }

    public void SetInput(bool set)
    {
        if (set)
        {
            input.Enable();
        }
        else
        {
            Debug.Log("Desactivando Input");
            input.Disable();
        }
    }

    public void ClearUp()
    {
        inputting = false;
        selectedGem = null;
    }

    public void CreateInfoPopup(Hero _hero, Mob _mob)
    {
        if (popInfo != null) { popInfo.Kill(); }
        popInfo = Instantiate(infoPop, manager.canvas.transform);
        popInfo.SetPopup(_hero, _mob, manager);
    }
    public void CreateGemPopup(Gem _gem)
    {
        if (popGem != null) { return; }
        popGem = Instantiate(gemPop, manager.canvas.transform);
        popGem.SetPopup(_gem, manager);
    }

    void HeroSelectionChange()
    {
        //identify selected
        HeroUI[] _heroes = allHeroesFolder.GetComponentsInChildren<HeroUI>();

        if (selectedFolder.childCount == 0)
        {
            for (int i = 0; i < _heroes.Length; i++)
            {
                _heroes[i].number = i;
            }
        }

        for (int i = 0; i < _heroes.Length; i++)
        {
            _heroes[i].Refresh(_heroes[i].hero == heroSelected ? selectedFolder : heroesFolder);
        }
    }
}
