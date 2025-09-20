using UnityEngine;

[System.Serializable]
public class ActorEffect {

    public string Name = "None";
    public int uses;

    public virtual float OnAttackEffect(float _attack) {
        return _attack;
    }
    public virtual float OnDefenseEffect(float _defense) {
        return _defense;
    }
    public virtual float OnHealEffect(float _amount) {
        return _amount;
    }
    public virtual float OnTakeDamageEffect(float _amount) {
        return _amount;
    }
    public virtual void OnTurnEffect() {
        AddUses(-1);
    }

    public void AddUses(int _amount) {
        uses += _amount;
    }

    public virtual ActorEffect CreateBuff() {
        return null;
    }
}
