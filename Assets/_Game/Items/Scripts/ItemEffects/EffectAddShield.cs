using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Effects/Add Shield")]
public class EffectAddShield : ItemEffect {

    public EffectShield effect;
    [Space]
    [Range(0f, 1f)]
    [SerializeField] float healthPercent;

    [SerializeField] bool allHeroes;
    [SerializeField] bool targetHero;
    [SerializeField] bool targetAndAdjecentHeroes;

    public EffectAddShield() {
        description = "Adds a shield to a hero or group of heroes by a flat amount.";
    }

    public override bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem) {
        if (_herotarg == null) { return false; }

        if (targetHero) {

            if (healthPercent > 0) {
                effect.shield = _herotarg.maxHealth * healthPercent;
                effect.maxShield = _herotarg.maxHealth * healthPercent;
            }
            _herotarg.AddEffect(effect);
            return true;
        }
        if (allHeroes) {
            if (_mng == null) { return false; }
            Hero[] _heros = _mng.GetHeros();

            for (int i = 0; i < _heros.Length; i++) {
                if (_heros[i] != null) {
                    if (healthPercent > 0) {
                        effect.shield = _heros[i].maxHealth * healthPercent;
                        effect.maxShield = _heros[i].maxHealth * healthPercent;
                    }
                    _heros[i].AddEffect(effect);
                }
            }
            return true;
        }
        return false;
    }
}
