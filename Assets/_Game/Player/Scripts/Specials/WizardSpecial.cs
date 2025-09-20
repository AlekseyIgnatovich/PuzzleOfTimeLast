using UnityEngine;

[CreateAssetMenu(menuName = "My File/Heroes/Specials/Wizard Special")]
public class WizardSpecial : HeroSpecial {

    public float specialMultiplier;
    [SerializeField] HurtTrigger hurtAnimation = HurtTrigger.Wizard;

    public WizardSpecial() {
        Name = "Force Link";
        description = "Deals damage to a target enemy, or heals an ally.";
    }

    public override bool ApplySpecial(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Hero _hero) {
        if (_hero == null) { return false; }

        if (_mobtarg != null) {
            _mobtarg.StoreAttacks(_hero.GetSpecialAttack() * specialMultiplier, _hero.type, true, hurtAnimation);
            return true;
        }
        if (_herotarg != null) {
            _herotarg.AddHealth(_hero.GetSpecialAttack() * .5f * specialMultiplier);
            return true;
        }

        return false;
    }
}
