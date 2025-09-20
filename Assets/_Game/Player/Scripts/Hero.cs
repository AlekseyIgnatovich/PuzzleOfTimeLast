using UnityEngine;

public class Hero : MonoBehaviour
{

    [SerializeField] AudioData audioData;
    [SerializeField] ParticleSystem specialEffect;
    public HeroBase heroBase;
    [Space]
    public int index;
    [Space]
    public float maxHealth;
    public float health;
    public float attack;
    public float defense;
    [Space]
    public ElementType type;
    [Space]
    public Rarity rarity;
    [Space]
    public float specialPoints;
    public float specialMaxPoints;
    public float specialPercent;
    public int level;
    public float experience;
    [Space]
    public ActorEffect[] effects;
    [Space]
    AudioClip attackSFX;
    AudioClip attackSpecialSFX;
    AudioClip spawnSFX;
    AudioClip deathSFX;
    [Space]
    [SerializeField] Animator animator;
    [SerializeField] HeroUI heroUI;
    public Healthbar healthBar;
    public Healthbar specialBar;
    public Healthbar shieldBar;
    [SerializeField] GameObject specialGlow;
    [SerializeField] FloaterText floaterText;
    [SerializeField] GrayscaleEffectUI grayscaleEffectUI;

    [HideInInspector] public HeroCard card;
    bool attacking;
    Hero heroTarget;
    Mob mobTarget;
    GameplayManager manager;

    public void SetMe(HeroItem _hero, GameplayManager _mng)
    {
        index = _hero.index;
        level = _hero.level;
        type = _hero.type;
        rarity = _hero.rarity;
        experience = _hero.experience;

        card = heroBase.heroCards[index];
        UpdateStats();

        health = maxHealth;
        specialPoints = 0;
        specialMaxPoints = _hero.specialMaxPoints;

        effects = new ActorEffect[10];

        attackSFX = card.attackSFX;
        attackSpecialSFX = card.attackSpecialSFX;
        spawnSFX = card.spawnSFX;
        deathSFX = card.deathSFX;

        manager = _mng;

        heroUI.SetUI(card);
        healthBar.UpdateHealth(health, maxHealth, false);
        heroUI.UpdateAttack(attack);
        heroUI.UpdateDefense(defense);
        GetSpecialCharge(0);
        audioData.PlaySFX(spawnSFX);
        SetAsTarget(false);
        UpdateShield();
    }

    public void SpecialAttackEnemy(Hero _hero, Mob _mob)
    {
        if (attacking) { return; }
        print($"Started to apply special");
        attacking = true;
        heroTarget = _hero;
        mobTarget = _mob;
        animator.SetTrigger("specialattack");
    }
    public void SpecialApply()
    {
        HeroSpecial hsp = card.heroClass.special;
        print($"trying to apply special");
        if (hsp == null) { return; }
        if (hsp.ApplySpecial(manager, heroTarget, mobTarget, this))
        {
            print($"special applied");
            specialPoints = 0;
            GetSpecialCharge(0);//updates special ui in this case
            audioData.PlaySFX(attackSpecialSFX);
            manager.timerMobsCheck = 1.5f;
        }
    }
    public void AttackEnemy()
    {
        if (attacking) { return; }
        attacking = true;
        animator.SetTrigger("attack");
        audioData.PlaySFX(attackSFX);
    }
    public void FinishAttack()
    {
        attacking = false;
    }
    public void SpecialVFXExecute()
    {
        specialEffect.Play();
    }

    public void AddEffect(ActorEffect _effect)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i] == null)
            {
                effects[i] = _effect.CreateBuff();
                break;
            }
        }
        UpdateShield();
    }
    public float GetAttack()
    {
        float _atk = attack;
        ActorEffect eff;
        for (int i = 0; i < effects.Length; i++)
        {
            eff = effects[i];
            if (eff != null)
            {
                _atk = eff.OnAttackEffect(_atk);
                if (eff.uses <= 0) { effects[i] = null; }
            }
        }
        return _atk;
    }
    public float GetDefense()
    {
        float _def = defense;
        ActorEffect eff;
        for (int i = 0; i < effects.Length; i++)
        {
            eff = effects[i];
            if (eff != null)
            {
                _def = eff.OnDefenseEffect(_def);
                if (eff.uses <= 0) { effects[i] = null; }
            }
        }
        return _def;
    }
    public float ModifyDamageTaken(float _dmg)
    {
        ActorEffect eff;
        for (int i = 0; i < effects.Length; i++)
        {
            eff = effects[i];
            if (eff != null)
            {
                _dmg = eff.OnTakeDamageEffect(_dmg);
                if (eff.uses <= 0) { effects[i] = null; }
            }
        }
        return _dmg;
    }
    public float GetHealingPower(float _amount)
    {
        ActorEffect eff;
        for (int i = 0; i < effects.Length; i++)
        {
            eff = effects[i];
            if (eff != null)
            {
                _amount = eff.OnHealEffect(_amount);
                if (eff.uses <= 0) { effects[i] = null; }
            }
        }
        return _amount;
    }
    public void OnTurnEffects()
    {
        ActorEffect eff;
        for (int i = 0; i < effects.Length; i++)
        {
            eff = effects[i];
            if (eff != null)
            {
                eff.OnTurnEffect();
                if (eff.uses <= 0) { effects[i] = null; }
            }
        }
    }

    public float GetPower()
    {
        return Mathf.Floor((attack + defense + maxHealth) / 3);
    }


    void UpdateStats()
    {
        attack = card.attack + (card.heroClass.attackModifier * level);
        defense = card.defense + (card.heroClass.defenseModifier * level);
        maxHealth = card.maxHealth + (card.heroClass.healthModifier * level);
        specialPercent = card.specialPercent + (card.heroClass.specialModifier * level);
        if (specialPoints >= specialMaxPoints) { specialPoints = specialMaxPoints; }
    }
    public void GetExperience(float _amount)
    {
        bool levelled = false;
        float _roof;
        float _am;
        ExperienceLevels xpt = heroBase.settings.xpThreshold;
        int mlv = heroBase.settings.maxLevels[(int)rarity];

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
        if (levelled) { UpdateStats(); }
    }

    public void GetSpecialCharge(float _amount)
    {
        //print($"{heroBase.heroCards[index].Name} got {amount} special charge!");
        specialPoints = Mathf.Clamp(specialPoints + _amount, 0, specialMaxPoints); ;
        specialBar.UpdateHealth(specialPoints, specialMaxPoints, false);
        if (specialPoints >= specialMaxPoints)
        {

            if (GameManager.instance.isFirstSpecial)
            {
                GameManager.instance.isFirstSpecial = false;
                transform.SetSiblingIndex(transform.parent.childCount - 1);
                GameManager.instance.onBoardingManager.ActivateSpecialIntruction(gameObject);
            }

            Debug.Log("Se activo el especial");
        }
        specialGlow.SetActive(specialPoints >= specialMaxPoints);
    }
    public void TakeDamage(float attack)
    {
        float dmg = 0f;
        float _def = GetDefense();
        if (attack >= _def) { dmg = (attack * 2) - _def; } else { dmg = (attack * attack) / _def; }

        float _odmg = dmg;
        dmg = ModifyDamageTaken(dmg);

        FloaterText _ftx = Instantiate(floaterText, manager.canvas.transform);
        if (transform != null)
        {
            _ftx.transform.position = transform.position + (Random.value * .23f * new Vector3(1 * (Random.value > .5f ? 1 : -1), 1 * (Random.value > .5f ? 1 : -1), 0));
            _ftx.Setup(dmg > 0 ? dmg.ToString("F0") : "", 80, dmg == 0 ? Color.cyan : dmg == _odmg ? Color.white : dmg > _odmg ? Color.red : Color.gray);
            _ftx.gameObject.SetActive(false);
            manager.AddFloater(_ftx);
        }

        health -= dmg;
        if (health <= 0)
        {
            audioData.PlaySFX(deathSFX);
            GameManager.instance.currentScreen.GetComponent<GameplayManager>().RegisterHeroDeath();
            Destroy(gameObject);
            return;
        }
        healthBar.UpdateHealth(health, maxHealth, false);
        UpdateShield();
        animator.SetTrigger("hurt");
        SetAsTarget(false);
        if (health < 150 && health > 0)
        {
            grayscaleEffectUI.ApplyGrayscale();
            grayscaleEffectUI.StartHurtGlowEffect();
            GameManager.instance.onBoardingManager.ActivateHealingInstruction(gameObject);
        }
    }
    public void AddHealth(float _amount)
    {
        Debug.Log("Giving Health to player");
        health += GetHealingPower(_amount);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health >= 150)
        {
            grayscaleEffectUI.RestoreColor();
            grayscaleEffectUI.StopHurtGlowEffect();
        }
        healthBar.UpdateHealth(health, maxHealth, false);
    }
    public void SetAsTarget(bool _set)
    {
        heroUI.targetMark.SetActive(_set);
    }

    void UpdateShield()
    {
        EffectShield shld;
        float am = 0f;
        float mam = 0f;
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i] is EffectShield)
            {
                shld = (EffectShield)effects[i];
                am += shld.shield;
                mam += shld.maxShield;
            }
        }
        shieldBar.UpdateHealth(am, mam, false);
    }

    public float GetSpecialAttack()
    {
        return attack * specialPercent;
    }
}
