using UnityEngine;

[System.Serializable]
public class EffectShield : ActorEffect {

    public float shield;
    public float maxShield;

    [SerializeField] bool depleteOnUse;

    public override float OnTakeDamageEffect(float _dmg) {
        shield -= _dmg;
        if (shield < 0) {
            uses = 0;
            return Mathf.Abs(shield);
        }
        return 0f;
    }

    public override void OnTurnEffect() {}

    public override ActorEffect CreateBuff() {
        EffectShield _neweffect = new EffectShield();
        _neweffect.maxShield = maxShield;
        _neweffect.shield = shield;
        _neweffect.depleteOnUse = depleteOnUse;
        _neweffect.uses = uses;
        _neweffect.Name = Name;

        Debug.Log($"Got Shield Buff");
        return _neweffect;
    }
}
