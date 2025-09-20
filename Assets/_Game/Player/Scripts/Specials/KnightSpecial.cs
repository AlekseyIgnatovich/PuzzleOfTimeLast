using UnityEngine;

[CreateAssetMenu(menuName = "My File/Heroes/Specials/Knight Special")]
public class KnightSpecial : HeroSpecial {

    public float specialMultiplier;
    [SerializeField] HurtTrigger hurtAnimation = HurtTrigger.Knight;

    public KnightSpecial() {
        Name = "Clean Sweep";
        description = "Deals damage to a target and its adjecent allies.";
    }

    public override bool ApplySpecial(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Hero _hero) {
        if ((_mobtarg == null) || (_hero == null) || (_mng == null)) { return false; }

        FieldSide tfs = _mobtarg.spawnPoint.fieldSide;
        Mob[] _mobs = _mng.GetMobs();
        float dmg = _hero.GetSpecialAttack() * specialMultiplier;

        _mobtarg.StoreAttacks(dmg, _hero.type, true, hurtAnimation);

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
        return true;
    }
}
