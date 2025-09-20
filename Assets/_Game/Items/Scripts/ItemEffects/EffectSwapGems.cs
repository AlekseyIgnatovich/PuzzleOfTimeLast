using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Effects/Swap Gems")]
public class EffectSwapGems : ItemEffect {

    public EffectSwapGems() {
        description = "Swaps the positions of two Gems.";
    }

    public override bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem) {

        if (_gem != null) {
            _mng.StartGemsSwapSelecting(_gem);
            return true;
        }

        return false;
    }

}
