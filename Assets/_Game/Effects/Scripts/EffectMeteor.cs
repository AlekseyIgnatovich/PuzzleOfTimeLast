using UnityEngine;

public class EffectMeteor : ItemEntity {

    [SerializeField] HurtTrigger hurtAnimation = HurtTrigger.Default;

    public override void ApplyEffect() {
        if (mob == null) { return; }

        if (flatAmount > 0) {
            mob.StoreAttacks(flatAmount, ElementType.None, true, hurtAnimation);
            return;
        }
        if (percentAmount > 0) {
            mob.StoreAttacks(mob.maxHealth * percentAmount, ElementType.None, true, hurtAnimation);
            return;
        }
    }

}
