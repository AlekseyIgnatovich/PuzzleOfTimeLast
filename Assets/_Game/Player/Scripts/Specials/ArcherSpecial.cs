using UnityEngine;

[CreateAssetMenu(menuName ="My File/Heroes/Specials/Archer Special")]
public class ArcherSpecial : HeroSpecial {

    public float specialMultiplier;
    [SerializeField] HurtTrigger hurtAnimation = HurtTrigger.Archer;

    public ArcherSpecial() {
        Name = "True Shot";
        description = "Deals high damage to a target.";
    }

    public override bool ApplySpecial(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Hero _hero) {
        if ((_mobtarg == null) || (_hero == null)) { return false; }

        _mobtarg.StoreAttacks(_hero.GetSpecialAttack() * specialMultiplier, _hero.type, true, hurtAnimation);
        return true;
    }
}
