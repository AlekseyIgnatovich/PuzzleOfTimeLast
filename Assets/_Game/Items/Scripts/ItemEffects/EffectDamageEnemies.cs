using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Effects/Damage Enemy")]
public class EffectDamageEnemies : ItemEffect {

    public float flatAmount;
    [Range(0, 1f)]
    public float percentAmount;
    [Space]
    [SerializeField] bool allEnemies;
    [SerializeField] bool targetEnemy;
    [SerializeField] bool targetAndAdjecentEnemies;
    [Space]
    [SerializeField] HurtTrigger hurtAnimation = HurtTrigger.Default;

    public EffectDamageEnemies() {
        description = "Damages an enemy or group of enemies by flat or percent amount.";
    }

    public override bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem) {
        if (_mobtarg == null) { return false; }

        if (targetEnemy) {
            if (_mobtarg == null) { return false; }

            if (entity != null) {
                Instantiate(entity, _mobtarg.spawnPoint.transform.position, Quaternion.identity).Setup(_herotarg, _mobtarg, _mng, flatAmount, percentAmount);
            } else {
                if (flatAmount > 0) {
                    _mobtarg.StoreAttacks(flatAmount, ElementType.None, true, hurtAnimation);
                }
                if (percentAmount > 0) {
                    _mobtarg.StoreAttacks(_mobtarg.maxHealth * percentAmount, ElementType.None, true, hurtAnimation);
                }
            }
        }
        if (allEnemies) {
            if (_mng == null) { return false; }

            Mob[] _mobs = _mng.GetMobs();

            for (int i = 0; i < _mobs.Length; i++) {
                if (_mobs[i] != null) {
                    if (entity != null) {
                        Instantiate(entity, _mobs[i].spawnPoint.transform.position, Quaternion.identity).Setup(_herotarg, _mobs[i], _mng, flatAmount, percentAmount);
                    } else {
                        if (flatAmount > 0) {
                            _mobs[i].StoreAttacks(flatAmount, ElementType.None, true, hurtAnimation);
                        }
                        if (percentAmount > 0) {
                            _mobs[i].StoreAttacks(_mobs[i].maxHealth * percentAmount, ElementType.None, true, hurtAnimation);
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }
}
