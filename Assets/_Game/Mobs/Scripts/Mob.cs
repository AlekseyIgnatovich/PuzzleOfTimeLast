using UnityEngine;

public class Mob : MonoBehaviour
{

    public MobCard card;
    public int[] columns;
    [Space]
    public int level;
    public float health;
    public float maxHealth;
    [Space]
    public int attack;
    public int defense;
    public int turnsCount;
    [Space]
    public MobAbility[] abilities;
    [Space]
    public float value;
    public ElementType type;
    public Resistance[] resistance;
    [Space]
    public ActorEffect[] effects;
    [Space]
    AudioClip attackSFX;
    AudioClip spawnSFX;
    AudioClip deathSFX;
    [HideInInspector] public int counter;
    float timer;
    [HideInInspector] public SpawnPoint spawnPoint;
    [Space]
    [SerializeField] GameObject body;
    [SerializeField] Transform centerPoint;
    [SerializeField] AudioData audioData;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer picture;
    [SerializeField] SpriteRenderer sprite;
    [Space]
    [SerializeField] AudioClip targetHeroSFX;
    [SerializeField] GameObject effectSpawn;
    [SerializeField] GameObject effectHit;
    [SerializeField] FloaterText floaterText;


    [Space]

    float storedAttacks;
    float timerTakeDamage;
    Hero heroTarget;

    int selectedAbility;

    HurtTrigger hurtAnimation = HurtTrigger.Default;

    private void Start()
    {
        spawnPoint.UpdateHealth(health, maxHealth);
        Instantiate(effectSpawn, centerPoint.position, Quaternion.identity);
    }
    private void Update()
    {
        if (timerTakeDamage > 0)
        {
            timerTakeDamage -= Time.deltaTime;
            if (timerTakeDamage <= 0)
            {
                TakeDamage(storedAttacks);
                storedAttacks = 0;
                animator.SetFloat("hurtset", (float)hurtAnimation);
                animator.SetTrigger("hurt");
            }
        }
    }

    public void SetMob(SpawnPoint spnpt, MobCard _mob)
    {
        card = _mob;
        spawnPoint = spnpt;
        columns = spawnPoint.columns;

        level = 1;
        health = _mob.health;
        maxHealth = _mob.maxHealth;
        attack = _mob.attack;
        defense = _mob.defense;
        turnsCount = _mob.turnsCount;

        type = spawnPoint.type;
        resistance = new Resistance[spawnPoint.resistance.Length];
        for (int i = 0; i < resistance.Length; i++)
        {
            resistance[i] = spawnPoint.resistance[i];
        }

        abilities = new MobAbility[_mob.abilities.Length];
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = new MobAbility();
            abilities[i].Name = _mob.abilities[i].Name;
            abilities[i].chanse = _mob.abilities[i].chanse;
            abilities[i].condition = _mob.abilities[i].condition;
            abilities[i].type = _mob.abilities[i].type;
            abilities[i].uses = _mob.abilities[i].uses;
            abilities[i].usesTake = _mob.abilities[i].usesTake;
            abilities[i].applyValue = _mob.abilities[i].applyValue;
            abilities[i].conditionValue = _mob.abilities[i].conditionValue;
        }

        value = _mob.value;

        effects = new ActorEffect[10];

        picture.sprite = _mob.sprite;

        animator.runtimeAnimatorController = _mob.animator;

        attackSFX = _mob.attackSFX;
        spawnSFX = _mob.spawnSFX;
        deathSFX = _mob.deathSFX;

        counter = turnsCount;
        spawnPoint.UpdateCounter(counter);
        spawnPoint.UpdateHealth(health, maxHealth);
        audioData.PlaySFX(spawnSFX);

        animator.Play("idle", 0, 0f);
        picture.size = _mob.size;

        spawnPoint.sprite.enabled = true;
        spawnPoint.UpdatePosition();

        if (_mob.isHero)
        {
            body.SetActive(true);
            sprite.sprite = _mob.sprite;
            picture.enabled = false;
        }
    }

    public bool OnColumn(int column)
    {
        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i] == column) { return true; }
        }
        return false;
    }
    public void StoreAttacks(float _attack, ElementType _type, bool _nohero, HurtTrigger _hurt)
    {
        float _ogatk = _attack;

        if ((resistance.Length > 0) && (_type != ElementType.None))
        {
            _attack = ModifyAttack(_attack, _type);
        }

        //GameManager.instance.titansData.SaveTitanHealth(health);

        storedAttacks += _attack;
        print($"{card.Name} recived {_attack}DMG / type[{type}] dmg type[{_type}]");
        if (storedAttacks > 0) { timerTakeDamage = .1f; }

        FloaterText _ftx = Instantiate(floaterText, spawnPoint.manager.canvas.transform);
        _ftx.transform.position = centerPoint.position + (Random.value * .23f * new Vector3(1 * (Random.value > .5f ? 1 : -1), 1 * (Random.value > .5f ? 1 : -1), 0));
        _ftx.Setup(_attack > 0 ? _attack.ToString("F0") : "", 80, _attack == 0 ? Color.cyan : _attack == _ogatk ? Color.white : _attack > _ogatk ? Color.red : Color.gray);
        _ftx.gameObject.SetActive(false);
        spawnPoint.manager.AddFloater(_ftx);

        hurtAnimation = _hurt;

        if (_hurt != HurtTrigger.Default)
        {
            spawnPoint.SpawnSpecialSlashEffect();
        }
    }
    float ModifyAttack(float _attack, ElementType _type)
    {

        for (int i = 0; i < resistance.Length; i++)
        {
            if (resistance[i].type == _type)
            {
                return _attack * resistance[i].modifier;
            }
        }

        return _attack;
    }
    public void TakeDamage(float attack)
    {
        float dmg = 0f;
        float _def = GetDefense();
        if (attack >= _def) { dmg = (attack * 2) - _def; }
        else { dmg = (attack * attack) / _def; }

        health -= dmg;
        if (health <= 0)
        {
            spawnPoint.sprite.enabled = false;
            spawnPoint.manager.AccumulateXP(value);
            spawnPoint.MyMob = null;
            Destroy(gameObject);
        }
        spawnPoint.UpdateHealth(health, maxHealth);
        Instantiate(effectHit, centerPoint.position, Quaternion.identity);
    }
    public void DepleteTurnCounter()
    {
        if (counter > 0)
        {
            counter--;
            spawnPoint.UpdateCounter(counter);
        }
    }
    public void ResetTurnCounter()
    {
        if (counter <= 0)
        {
            counter = turnsCount;
            spawnPoint.UpdateCounter(counter);
        }
    }
    public void AttackHero(Hero _hero)
    {
        heroTarget = _hero;
        animator.SetTrigger("attack");
        audioData.PlaySFX(targetHeroSFX);
    }
    public void ApplyAttack()
    {
        heroTarget.TakeDamage(attack);
        audioData.PlaySFX(attackSFX);
    }
    public void UseAbility()
    {
        abilities[selectedAbility].uses -= abilities[selectedAbility].usesTake;
        switch (abilities[selectedAbility].type)
        {
            case AbilityType.HealthRestore:
                animator.SetTrigger("heal");
                break;
            case AbilityType.MobResurrect:
                animator.SetTrigger("resurrect");
                break;
        }
    }
    public bool CheckAbilities()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i].ConditionCheck(this, spawnPoint.manager))
            {
                selectedAbility = i;
                return true;
            }
        }
        return false;
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


    public void HealSelf()
    {
        health = maxHealth;
        spawnPoint.UpdateHealth(health, maxHealth);
    }
    public void ResurrectMob()
    {
        GameplayManager _manager = spawnPoint.manager;
        Mob[] _mobs = _manager.GetMobs();
        for (int i = 0; i < _mobs.Length; i++)
        {
            if (_mobs[i] == null) { _manager.SpawnMob(i, _manager.waveCount - 1); break; }
        }
    }
}
