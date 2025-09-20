using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class WorldMapManager : Manager
{

    public LevelsDatabase data;
    public StagePopup stagePopup;
    [Space]
    [SerializeField] ScrollRect scroller;
    [SerializeField] RectTransform scaler;
    [SerializeField] RectTransform map;
    [SerializeField] RectTransform stages;
    [SerializeField] float scaleRate;

    PlayerInput input;

    [SerializeField] bool inputting;
    [SerializeField] bool moving;
    [SerializeField] float distance;
    [SerializeField] Vector2 position1;
    [SerializeField] Vector2 position2;

    [Space]
    [SerializeField] Vector2 scrollValue;
    [SerializeField] float moveSensitivity;

    [SerializeField] Vector2 originPosition;
    [SerializeField] float mapWidth;
    [SerializeField] float mapHeight;

    [SerializeField] GameObject instruction;
    [SerializeField] GameObject homeBtn;

    private void Awake()
    {
        input = new PlayerInput();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        input.Player.Touch1Press.started += Touch1Press;
        input.Player.Touch1Press.canceled += Touch1Press;
        input.Player.Touch2Press.started += Touch2Press;
        input.Player.Touch2Press.canceled += Touch2Press;

        mapWidth = map.sizeDelta.x;
        mapHeight = map.sizeDelta.y;

        CenterMapOnStage();

        gameManager.levelsData = data;
        if (gameManager.isFirstTime && instruction != null)
        {
            instruction.SetActive(true);
            homeBtn.SetActive(false);
            //scroller.enabled = false;
            gameManager.bottomTabObj.SetActive(false);
        }
        else if (!gameManager.isFirstTime && gameManager.isFirstLevelUp)
        {
            gameManager.bottomTabController.SetHeroInstruction();
        }
        //data.Load();
    }

    private void Update()
    {
        if (gameManager.isFirstTime) return;
        scrollValue = input.Player.MyScroll.ReadValue<Vector2>();

        Vector2 _pos = map.anchoredPosition;

        if (scrollValue != Vector2.zero)
        {
            float _scale = scaler.localScale.x;

            _scale += scrollValue.y * scaleRate;

            _scale = Mathf.Clamp(_scale, 1, 2);

            scaler.localScale = Vector2.one * _scale;
        }

        if (inputting)
        {
            position1 = input.Player.Touch1Position.ReadValue<Vector2>();
            position2 = input.Player.Touch2Position.ReadValue<Vector2>();

            float _dis = Vector2.Distance(position1, position2);

            //set scale
            float _scale = scaler.localScale.x;

            _scale += (_dis - distance) * scaleRate;

            _scale = Mathf.Clamp(_scale, 1, 2);

            scaler.localScale = Vector2.one * _scale;

            distance = _dis;
        }
        else
        {
            //moving
            if (moving)
            {
                position1 = input.Player.Touch1Position.ReadValue<Vector2>();

                if (originPosition != position1)
                {

                    _pos += (position1 - originPosition) / scaler.localScale;

                    originPosition = position1;
                }
            }
        }

        float _sw = Mathf.Max((mapWidth * .5f) - ((Screen.width * .5f) / scaler.localScale.x), 0);
        float _sh = Mathf.Max((mapHeight * .5f) - ((Screen.height * .5f) / scaler.localScale.x), 0);

        _pos.x = Mathf.Clamp(_pos.x, -_sw, _sw);
        _pos.y = Mathf.Clamp(_pos.y, -_sh, _sh);

        map.anchoredPosition = _pos;
    }

    public override void Initialize(GameManager _manager)
    {
        gameManager = _manager;
        canvas.worldCamera = Camera.main;
        stagePopup.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        gameManager.SetState(GameState.Gameplay);
    }

    void Touch1Press(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            moving = true;
            position1 = input.Player.Touch1Position.ReadValue<Vector2>();
            originPosition = position1;
        }
        if (_context.canceled)
        {
            moving = false;
        }
    }
    void Touch2Press(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            inputting = true;
            scroller.enabled = false;
            distance = Vector2.Distance(
                input.Player.Touch1Position.ReadValue<Vector2>(),
                input.Player.Touch2Position.ReadValue<Vector2>());
        }
        if (_context.canceled)
        {
            inputting = false;
            scroller.enabled = true;
        }
    }

    public void SelectStage(int _sel)
    {
        data.SelectLevel(_sel);
        gameManager.SetState(GameState.Loadout);
        //stagePopup.gameObject.SetActive(true);
        //stagePopup.UpdateInfo();
    }

    public void GoToMainMenu()
    {
        gameManager.SetState(GameState.MainMenu);
    }

    public void GoToHome()
    {
        gameManager.ReturnToHome();
    }
    public void GoToMapSelect()
    {
        gameManager.SetState(GameState.MapSelect);
    }

    void CenterMapOnStage()
    {
        RectTransform _stage = stages.GetChild(Mathf.Clamp(data.progress, 0, stages.childCount - 1)).GetComponent<RectTransform>();

        //set position
        print($"map anchored position {map.anchoredPosition}");
        print($"stage anchored position {_stage.anchoredPosition}");
        print($"stage name {_stage.name}");
        scaler.localScale = Vector2.one * 2;
        map.anchoredPosition = -_stage.anchoredPosition;
    }
}
