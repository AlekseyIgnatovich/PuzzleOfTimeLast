using UnityEngine;

[CreateAssetMenu(menuName = "My File/Heroes/Specials/Guardian Special")]
public class GuardianSpecial : HeroSpecial {

    public float specialMultiplier;
    [Space]
    public EffectAttack attackBuff;
    public EffectDefense defenseNerf;
    [Space]
    [SerializeField] HurtTrigger hurtAnimation = HurtTrigger.Guardian;

    public GuardianSpecial() {
        Name = "Grave Sweep";
        description = "Deals damage to a target and its adjecent allies, also buffs or nerfs enemies, chosen at random.";
    }

    public override bool ApplySpecial(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Hero _hero) {
        if ((_mobtarg == null) || (_hero == null) || (_mng == null)) { return false; }

        FieldSide tfs = _mobtarg.spawnPoint.fieldSide;
        Mob[] _mobs = _mng.GetMobs();
        float dmg = _hero.GetSpecialAttack() * specialMultiplier;

        _mobtarg.StoreAttacks(dmg, _hero.type, true, hurtAnimation);

        //apply damage
        if (tfs == FieldSide.Mid) {
            for (int i = 0; i < _mobs.Length; i++) {
                if ((_mobs[i] != null) && (_mobs[i].spawnPoint.fieldSide != FieldSide.Mid)) {
                    _mobs[i].StoreAttacks(dmg, _hero.type, true, hurtAnimation);
                }
            }
        } else {
            for (int i = 0; i < _mobs.Length; i++) {
                if ((_mobs[i] != null) && (_mobs[i].spawnPoint.fieldSide == FieldSide.Mid)) {
                    _mobs[i].StoreAttacks(dmg, _hero.type, true, hurtAnimation);
                }
            }
        }

        //apply buff or nerf
        if (Random.value > Random.value) {
            //buff allies
            Hero[] _heros = _mng.GetHeros();
            for (int i = 0; i < _heros.Length; i++) {
                if ((_heros[i] != null)) {
                    _heros[i].AddEffect(attackBuff);
                }
            }
        } else {
            //nerf enemis
            for (int i = 0; i < _mobs.Length; i++) {
                if ((_mobs[i] != null)) {
                    _mobs[i].AddEffect(defenseNerf);
                }
            }
        }
        return true;
    }
}
