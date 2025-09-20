using UnityEngine;

[CreateAssetMenu(menuName = "My File/Items/Effects/Restore Special")]
public class EffectRestoreSpecial : ItemEffect {

    public float flatAmount;
    [Range(0, 1f)]
    public float percentAmount;

    [SerializeField] bool allHeroes;
    [SerializeField] bool targetHero;
    [SerializeField] bool targetAndAdjecentHeroes;

    public EffectRestoreSpecial() {
        description = "Restores special on a hero or group of heroes by flat or percent amount.";
    }

    public override bool ApplyEffect(GameplayManager _mng, Hero _herotarg, Mob _mobtarg, Gem _gem) {
        if (_herotarg == null) { return false; }

        if (targetHero) {

            if (flatAmount > 0) {
                _herotarg.GetSpecialCharge(flatAmount);
                return true;
            }
            if (percentAmount > 0) {
                _herotarg.GetSpecialCharge(_herotarg.specialMaxPoints * percentAmount);
                return true;
            }
        }
        if (allHeroes) {
            if (_mng == null) { return false; }
            Hero[] _heros = _mng.GetHeros();

            for (int i = 0; i < _heros.Length; i++) {
                if (_heros[i] != null) {
                    if (flatAmount > 0) {
                        _heros[i].GetSpecialCharge(flatAmount);
                    }
                    if (percentAmount > 0) {
                        _heros[i].GetSpecialCharge(Mathf.Floor(_heros[i].specialPercent * _heros[i].attack) * percentAmount);
                    }
                }
            }
            return true;
        }
        return false;
    }
}
