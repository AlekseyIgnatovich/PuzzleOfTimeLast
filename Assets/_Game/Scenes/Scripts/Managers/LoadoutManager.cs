using UnityEngine;

public class LoadoutManager : Manager
{

    [SerializeField] HeroBase heroBase;
    [SerializeField] ItemsData itemsData;
    [SerializeField] LoadoutPopup popup;
    [Space]
    [SerializeField] Transform heroesContent;
    [SerializeField] Transform itemsContent;
    [SerializeField] HeroLoad heroLoad;
    [SerializeField] ItemLoad itemLoad;
    [SerializeField] GameObject[] instructions;
    [SerializeField] GameObject[] tutorialImages;
    [SerializeField] bool resetTutorial;

    bool isShowingTutorial;
    int instructionIndex;

    private void Start()
    {
        CreateHeroes();
        CreateItems();
        if (resetTutorial) ResetInstructions();

        if (gameManager.isFirstGame && !gameManager.hasOpenBefore)
        {
            instructionIndex = 0;
            gameManager.hasOpenBefore = true;
        }
        else
        {
            instructionIndex = PlayerPrefs.GetInt("LoadOut_Index", 0);
        }

        if (gameManager.isFirstTime && instructionIndex < instructions.Length)
        {
            isShowingTutorial = true;
            instructions[instructionIndex].SetActive(true);
            for (int i = 0; i < tutorialImages.Length; i++)
            {
                tutorialImages[i].SetActive(i == instructionIndex);
            }
            //ShowNextInstruction();
        }
    }

    public void ResetInstructions()
    {
        PlayerPrefs.SetInt("LoadOut_Index", 0);
    }

    public void ShowNextInstruction()
    {
        if (!isShowingTutorial) return;

        instructionIndex++;
        PlayerPrefs.SetInt("LoadOut_Index", instructionIndex);
        if (instructionIndex >= instructions.Length)
        {
            isShowingTutorial = false;
        }
    }

    void CreateHeroes()
    {
        HeroItem[] _heroes = heroBase.hero;

        for (int i = 0; i < _heroes.Length; i++)
        {
            if (_heroes[i] != null)
            {
                if (_heroes[i].selected)
                {
                    Instantiate(heroLoad, heroesContent).Setup(_heroes[i]);
                }
            }
        }
    }
    void CreateItems()
    {
        ItemObject[] _items = new ItemObject[itemsData.items.Count];

        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = itemsData.items[i];
            if (_items[i] != null)
            {
                if (_items[i].selected)
                {
                    Instantiate(itemLoad, itemsContent).Setup(_items[i]);
                }
            }
        }
    }

    public void SelectHeroes()
    {
        gameManager.SetState(GameState.HeroSelect);
    }
    public void SelectItems()
    {

        gameManager.SetState(GameState.ItemSelect);
    }
    public void StartGame()
    {
        if (heroesContent.childCount == 0) { popup.Setup(); return; }

        // if (gameManager.isFirstTime)
        // {
        //     gameManager.isFirstTime = false;
        //     //PlayerPrefs.SetInt("Tutotial_Completed", 1);
        // }


        gameManager.SetState(GameState.Gameplay);
    }
}
