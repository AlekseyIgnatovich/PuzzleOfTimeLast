using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroSlot : MonoBehaviour
{

    [SerializeField] HeroBase heroBase;
    [SerializeField] GemData gemData;

    [SerializeField] Image frame;
    [SerializeField] Image label;
    [SerializeField] Image heroImage;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textPower;
    [SerializeField] GameObject battleMark;
    [SerializeField] GameObject sacrificeMark;
    [SerializeField] TextMeshProUGUI rarityTMP;
    [SerializeField] TextMeshProUGUI lvTMP;

    public bool sacrifice;
    public bool forbattle;
    public int index;//index in inventory

    public void SetData(int _index)
    {
        index = _index;
        sacrifice = true;

        HeroItem _hero = heroBase.hero[index];
        HeroCard _card = heroBase.heroCards[_hero.index];

        textName.text = $"{_card.Name}";
        textPower.text = _card.GetPower(_hero.level).ToString("F0");
        heroImage.sprite = _card.gameSprite;
        rarityTMP.text = _card.rarity.ToString();
        frame.sprite = heroBase.GetElementFrame(_card.type);
        label.sprite = heroBase.GetElementLabel(_card.type);
        Debug.Log("Hero: " + _card.Name + "Level " + _hero.level);
        lvTMP.text = (_hero.level + 1).ToString();

        forbattle = _hero.selected;
        battleMark.SetActive(forbattle);
        ToggleForSacrifice();
    }

    public HeroItem GetHero()
    {
        return heroBase.hero[index];
    }

    public void SelectForBattle()
    {
        if (!heroBase.SelectForBattle(index, forbattle))
        {
            Debug.Log("Has selected max heroes amount");
            forbattle = false;
            battleMark.SetActive(forbattle);
        }
    }
    public void ToggleForBattle()
    {
        forbattle = !forbattle;
        battleMark.SetActive(forbattle);
        //heroBase.Save();
    }
    public void SelectHero()
    {
        heroBase.SelectHero(index);
        Debug.Log("Select");
    }
    public void ToggleForSacrifice()
    {
        sacrifice = !sacrifice;
        sacrificeMark.SetActive(sacrifice);
    }

    public float GetPower()
    {
        return heroBase.hero[index].GetPower();
    }
}
