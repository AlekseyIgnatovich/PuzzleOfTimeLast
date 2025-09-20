using UnityEngine;

[System.Serializable]
public class EffectDefense : ActorEffect {

    [SerializeField] float flatDefenseModifier;
    [SerializeField] float multiDefenseModifier;

    [SerializeField] bool depleteOnUse;

    public override float OnDefenseEffect(float _defense) {
        if (depleteOnUse) { AddUses(-1); }
        return (_defense + flatDefenseModifier) * multiDefenseModifier;
    }

    public override ActorEffect CreateBuff() {
        EffectDefense _neweffect = new EffectDefense();
        _neweffect.flatDefenseModifier = flatDefenseModifier;
        _neweffect.multiDefenseModifier = multiDefenseModifier;
        _neweffect.depleteOnUse = depleteOnUse;
        _neweffect.uses = uses;
        _neweffect.Name = Name;

        Debug.Log($"Got Defense Buff");
        return _neweffect;
    }
}
