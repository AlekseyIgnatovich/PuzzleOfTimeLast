using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Effects/Heal Ally")]
public class EffectHealAlly : ItemEffect{

    public float flatAmount;
    [Range(0, 1f)]
    public float percentAmount;

    [SerializeField] bool allHeroes;
    [SerializeField] bool targetHero;
    [SerializeField] bool targetAndAdjecentHeroes;

    public EffectHealAlly() {
        description = "Heals a hero or group of heroes by flat or percent amount.";
    }

    public override bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem) {
        if (_herotarg == null) { return false; }

        if (targetHero) {

            if (entity != null) {
                Instantiate(entity, _herotarg.transform.position, Quaternion.identity);
            }
            if (flatAmount > 0) {
                _herotarg.AddHealth(flatAmount);
                return true;
            }
            if (percentAmount > 0) {
                _herotarg.AddHealth(_herotarg.maxHealth * percentAmount);
                return true;
            }
        }
        if (allHeroes) {
            if (_mng == null) { return false; }
            Hero[] _heros = _mng.GetHeros();

            for (int i = 0; i < _heros.Length; i++) {
                if (_heros[i] != null) {
                    if (flatAmount > 0) {
                        _heros[i].AddHealth(flatAmount);
                    }
                    if (percentAmount > 0) {
                        _heros[i].AddHealth(_heros[i].maxHealth * percentAmount);
                    }
                }
            }
            return true;
        }
        return false;
    }
}
