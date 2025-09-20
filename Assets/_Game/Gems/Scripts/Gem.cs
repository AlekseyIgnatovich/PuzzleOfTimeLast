using UnityEngine;
using TMPro;

public class Gem : MonoBehaviour
{
    [SerializeField] ElementType type;
    [SerializeField] AudioData audioData;
    [SerializeField] GemData data;
    [Space]
    public int number;
    public int column;

    public GameplayManager manager;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    [SerializeField] GameObject rim;
    [SerializeField] GameObject rimM4;
    [SerializeField] GameObject rimM5;
    [SerializeField] GameObject arrowsHorizontal;
    [SerializeField] GameObject arrowsVertical;
    [SerializeField] GameObject deadmark;

    public Gem gemToSwitchWith;
    float timer;
    [SerializeField] float timeMove;
    [HideInInspector] public Vector2 originalPosition;

    public bool dead;
    public int shiftCount;

    bool shifting;
    bool returnShift;
    Vector2 shiftPosition;
    ElementType originalType;

    public bool chosen;
    public Match match;
    public SwipeDirection swipe;

    [Space]
    [SerializeField] AudioClip spawnSFX;
    [SerializeField] TextMeshProUGUI swipetext;
    [Space]
    [SerializeField] GameObject[] matchEffect;
    [SerializeField] GameObject[] match4HRims;
    [SerializeField] GameObject[] match4VRims;
    [SerializeField] GameObject[] match5Rims;

    public ElementType Type
    {
        get { return type; }
        set { type = value; UpdateType(); }
    }

    private void Start()
    {
        originalPosition = transform.parent.position;
        dead = false;
        SetMatch(false);
        originalType = type;
    }

    private void Update()
    {
        if (gemToSwitchWith != null)
        {
            #region NOTEs
            //when we have a gem to switch with, we either move from our OP to other gem OP or we move from our OP to other gem OP.
            //the position is defined by the timer
            //at the end of the timer we check for a match or set to return to position if no match
            //and finally we just set the gem to either other gem or to our original setup
            #endregion
            if (timer > 0)
            {
                timer = Mathf.Max(timer - Time.deltaTime, 0);
            }

            if (returnShift)
            {
                transform.position = Vector2.Lerp(originalPosition, gemToSwitchWith.originalPosition, timer / timeMove);
                gemToSwitchWith.transform.position = Vector2.Lerp(gemToSwitchWith.originalPosition, originalPosition, timer / timeMove);
            }
            else
            {
                transform.position = Vector2.Lerp(gemToSwitchWith.originalPosition, originalPosition, timer / timeMove);
                gemToSwitchWith.transform.position = Vector2.Lerp(originalPosition, gemToSwitchWith.originalPosition, timer / timeMove);
            }

            if (timer <= 0)
            {
                if (returnShift)
                {
                    returnShift = false;
                    gemToSwitchWith.ClearSwipe();
                    gemToSwitchWith = null;
                }
                else
                {
                    if (manager.CheckForMatches())
                    {
                        transform.localPosition = Vector3.zero;
                        gemToSwitchWith.transform.localPosition = Vector3.zero;

                        UpdateGraphics();
                        gemToSwitchWith.UpdateGraphics();

                        gemToSwitchWith = null;
                    }
                    else
                    {
                        timer = timeMove;
                        returnShift = true;
                        SwitchParameters();
                    }
                }
                ClearSwipe();
            }
        }

        //this code is for when gems are falling in the grid, after a match has been made
        if (shifting)
        {
            if (timer > 0)
            {
                timer = Mathf.Max(timer - Time.deltaTime, 0);

                transform.position = Vector2.Lerp(originalPosition, shiftPosition, timer / timeMove);
                if (timer <= 0)
                {
                    shifting = false;
                    transform.localPosition = Vector3.zero;
                    ClearSwipe();
                }
            }
        }

        //swipetext.text = $"{swipe}";
    }

    public void SetMove(Gem _gem)
    {
        gemToSwitchWith = _gem;

        SwitchParameters();

        timer = timeMove;
        returnShift = false;
    }
    public void SetMatch(bool _match)
    {
        if (dead)
        {
            ClearMatchMarks();
            return;
        }
        else
        {
            if (_match)
            {
                match = (Match)Mathf.Max((int)match, 1);
                Debug.Log(match.ToString());

                Instantiate(matchEffect[(int)type], transform.position, Quaternion.identity);
                if (!chosen)
                {
                    dead = true;

                    if (match == Match.Match4)
                    {
                        if (swipe == SwipeDirection.Horizontal)
                        {
                            manager.Match4RollExecute(number);
                        }
                        if (swipe == SwipeDirection.Vertical)
                        {
                            manager.Match4ColumnExecute(column);
                        }
                    }
                    if (match == Match.Match5)
                    {
                        manager.Match5Execute(type);
                    }

                    animator.SetTrigger("dead");
                    //manager.AddHeroSpecial(type);
                    //manager.DamageColumn(column, type);
                }
                manager.AddHeroSpecial(type);
                manager.DamageColumn(column, type);
            }
        }

        chosen = false;
        UpdateGraphics();
    }

    public void SwitchParameters()
    {
        ElementType _type = type;
        Match _match = match;
        SwipeDirection _swipe = swipe;

        type = gemToSwitchWith.type;
        match = gemToSwitchWith.match;
        swipe = gemToSwitchWith.swipe;

        gemToSwitchWith.type = _type;
        gemToSwitchWith.match = _match;
        gemToSwitchWith.swipe = _swipe;
    }
    void ClearSwipe()
    {
        if (match < Match.Match4)
        {
            swipe = SwipeDirection.None;
        }
    }

    public void SetGem(int _type = -1, bool set = false)
    {
        data.SetGem(this, _type);
        SetMatch(set);
        originalType = type;
        //audioData.PlaySFX(spawnSFX);
    }
    public void SetChosen(int _matches, SwipeDirection _swipe)
    {
        chosen = true;
        dead = false;

        match = Match.None;
        if (_matches == 3) { match = Match.Match3; }
        if (_matches == 4) { match = Match.Match4; }
        if (_matches >= 5) { match = Match.Match5; }

        if (swipe == SwipeDirection.None) { swipe = _swipe; }

        UpdateGraphics();
    }
    public void UpdateGraphics()
    {
        if (match == Match.Match4)
        {
            if (swipe == SwipeDirection.Horizontal)
            {
                arrowsHorizontal.SetActive(true);
                arrowsVertical.SetActive(false);
            }
            if (swipe == SwipeDirection.Vertical)
            {
                arrowsHorizontal.SetActive(false);
                arrowsVertical.SetActive(true);
            }
        }

        SetSprite();

        rim.SetActive(false);
        rimM4.SetActive(match == Match.Match4);
        rimM5.SetActive(match == Match.Match5);
        //rim.SetActive(match == Match.Match3);
    }
    public void ClearMatchMarks()
    {
        spriteRenderer.sprite = data.GetSprite(type);
        match = Match.None;
        rim.SetActive(false);
        rimM4.SetActive(false);
        rimM5.SetActive(false);
        arrowsHorizontal.SetActive(false);
        arrowsVertical.SetActive(false);
    }
    public void ResetGem()
    {
        data.SetGem(this);
        dead = false;
        SetMatch(false);
        animator.Play("birth", 0, 0);
        originalType = type;
        rim.SetActive(false);
        ClearSwipe();
        ClearMatchMarks();
        UpdateGraphics();
    }
    public void ShiftCheck()
    {
        if (shiftCount > 0)
        {
            shifting = true;
            timer = timeMove;
            Vector2 pos = originalPosition;
            pos.y += shiftCount * .6f;
            shiftPosition = pos;
            transform.position = pos;
            shiftCount = 0;
        }
    }
    public void SetSprite()
    {
        switch (match)
        {
            case Match.Match4:
                spriteRenderer.sprite = data.GetSpriteM4(type);
                break;
            case Match.Match5:
                spriteRenderer.sprite = data.GetSpriteM5(type);
                break;
            default:
                spriteRenderer.sprite = data.GetSprite(type);
                break;
        }
    }

    public void SelectGem()
    {
        rim.SetActive(true);
    }

    public void ApplyMatch5()
    {
        match = Match.Match5;
        UpdateGraphics();
    }

    void UpdateType()
    {
        int s = (int)type;
        for (int i = 0; i < match5Rims.Length; i++)
        {
            match4HRims[i].SetActive(i == s);
            match4VRims[i].SetActive(i == s);
            match5Rims[i].SetActive(i == s);
        }
    }
}
