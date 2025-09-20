using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Effects/Shuffle Gems")]
public class EffectShuffleGems : ItemEffect {

    public EffectShuffleGems() {
        description = "Changes the position of all Gems on the board.";
    }

    public override bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem) {

        if (_gem != null) {
            _mng.ShuffleAllGems();
            return true;
        }

        return false;
    }

}
