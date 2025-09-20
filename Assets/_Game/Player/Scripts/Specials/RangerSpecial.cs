using UnityEngine;

[CreateAssetMenu(menuName = "My File/Heroes/Specials/Ranger Special")]
public class RangerSpecial : HeroSpecial {

    public float specialMultiplier;
    [SerializeField] HurtTrigger hurtAnimation = HurtTrigger.Ranger;

    public RangerSpecial() {
        Name = "Arrow Rain";
        description = "Deals damage to all enemies.";
    }

    public override bool ApplySpecial(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Hero _hero) {
        if ((_mobtarg == null) || (_hero == null) || (_mng == null)) { return false; }

        Mob[] _mobs = _mng.GetMobs();
        float dmg = _hero.GetSpecialAttack() * specialMultiplier;

        for (int i = 0; i < _mobs.Length; i++) {
            if (_mobs[i] != null) {
                _mobs[i].StoreAttacks(dmg, _hero.type, true, hurtAnimation);
            }
        }
        return true;
    }
}
