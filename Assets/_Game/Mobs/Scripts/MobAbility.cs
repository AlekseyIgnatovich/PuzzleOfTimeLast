using UnityEngine;

[System.Serializable]
public class MobAbility {

    public string Name;
    [Space]
    public int uses;
    public float chanse;
    [Space]
    public int usesTake;
    [Space]
    public Condition condition;
    public float conditionValue;
    [Space]
    public AbilityType type;
    public float applyValue;

    public bool ConditionCheck(Mob _mob, GameplayManager _manager) {
        if ((uses > 0) && (Random.value < chanse)) {

            bool _ck = false;
            switch (condition) {
                case Condition.HealthGreaterThen:
                    _ck = _mob.health > conditionValue;
                    break;
                case Condition.HealthLessThen:
                    _ck = _mob.health < conditionValue;
                    break;
                case Condition.HealthPercentGreaterThen:
                    _ck = (_mob.health / _mob.maxHealth) > conditionValue;
                    break;
                case Condition.HealthPercentLessThen:
                    _ck = (_mob.health / _mob.maxHealth) < conditionValue;
                    break;
                case Condition.DeadMob:
                    Mob[] _mobs = _manager.GetMobs();
                    for (int i = 0; i < _mobs.Length; i++) {
                        if (_mobs[i] == null) { _ck = true; break; }
                    }
                    break;
            }

            if (_ck) {
                return true;
            }
        }
        return false;
    }

}
