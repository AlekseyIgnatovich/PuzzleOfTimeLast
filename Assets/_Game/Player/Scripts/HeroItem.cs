using UnityEngine;

[System.Serializable]
public class HeroItem
{

    public int index;
    [Space]
    public int level;
    public float maxHealth;
    public float attack;
    public float defense;
    public ElementType type;
    [Space]
    public Rarity rarity;
    [Space]
    public float experience;
    public float specialMaxPoints;
    public float specialPercent;
    public bool selected;

    public bool dead;

    public void SetData(int _index, float maxhp, float atk, float def, ElementType _type, Rarity _rarity, int _lv, float xp, float _spmp, float _spperc, bool _sel)
    {
        index = _index;
        maxHealth = maxhp;
        attack = atk;
        defense = def;
        type = _type;
        rarity = _rarity;
        level = _lv;
        experience = xp;
        specialMaxPoints = _spmp;
        specialPercent = _spperc;
        if (!selected) selected = _sel;

        dead = false;
    }

    public void SetStats(Hero _hero)
    {
        level = _hero.level;
        experience = _hero.experience;
        maxHealth = _hero.maxHealth;
        attack = _hero.attack;
        defense = _hero.defense;
        specialPercent = _hero.specialPercent;
    }

    void UpdateStats(HeroCard _card)
    {
        attack = _card.attack + (_card.heroClass.attackModifier * level);
        defense = _card.defense + (_card.heroClass.defenseModifier * level);
        maxHealth = _card.maxHealth + (_card.heroClass.healthModifier * level);
    }
    public void GetExperience(HeroBase _hbase, HeroItem _item)
    {
        float _amount = _hbase.settings.rarityXP[(int)_item.rarity] + (_item.level * 50);
        bool levelled = false;
        float _roof;
        float _am;
        ExperienceLevels xpt = _hbase.settings.xpThreshold;
        int mlv = _hbase.settings.maxLevels[(int)rarity];

        if (level < mlv)
        {
            while (_amount > 0)
            {
                _roof = xpt.roof[level];
                _am = Mathf.Min(_amount, _roof - experience);
                _amount -= _am;
                experience += _am;
                if (experience >= _roof)
                {
                    level++;
                    levelled = true;
                }
            }
        }
        if (level >= mlv)
        {
            level = mlv;
            experience = xpt.roof[level];
        }
        if (levelled) { UpdateStats(_hbase.heroCards[index]); }
    }

    public float GetPower()
    {
        return Mathf.Floor((attack + defense + maxHealth) / 3);
    }
}
