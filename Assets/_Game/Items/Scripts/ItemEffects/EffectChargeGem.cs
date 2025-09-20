using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Effects/Charge Gem")]
public class EffectChargeGem : ItemEffect {

    public EffectChargeGem() {
        description = "Charges a selected Gem to Match 5.";
    }

    public override bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem) {

        if ((_gem != null) && (_gem.match != Match.Match5)) {
            _gem.ApplyMatch5();
            return true;
        }

        return false;
    }

}
