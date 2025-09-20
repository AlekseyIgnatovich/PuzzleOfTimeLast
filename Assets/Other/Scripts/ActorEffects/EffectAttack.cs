using UnityEngine;

[System.Serializable]
public class EffectAttack : ActorEffect {

    [SerializeField] float flatAttackModifier;
    [SerializeField] float multiAttackModifier;

    [SerializeField] bool depleteOnUse;

    public override float OnAttackEffect(float _attack) {
        if (depleteOnUse) { AddUses(-1); }
        return (_attack + flatAttackModifier) * multiAttackModifier;
    }

    public override ActorEffect CreateBuff() {
        EffectAttack _neweffect = new EffectAttack();
        _neweffect.flatAttackModifier = flatAttackModifier;
        _neweffect.multiAttackModifier = multiAttackModifier;
        _neweffect.depleteOnUse = depleteOnUse;
        _neweffect.uses = uses;
        _neweffect.Name = Name;

        Debug.Log($"Got Attack Buff");
        return _neweffect;
    }
}
