using UnityEngine;

public class ItemEntity : MonoBehaviour {

    [HideInInspector] public GameplayManager manager;
    [HideInInspector] public Hero hero;
    [HideInInspector] public Mob mob;

    public float flatAmount;
    [Range(0, 1f)]
    public float percentAmount;

    public void Setup(Hero _hero, Mob _mob, GameplayManager _manager, float _flatamount, float _percentamount) {
        manager = _manager;
        hero = _hero;
        mob = _mob;
        flatAmount = _flatamount;
        percentAmount = _percentamount;
    }

    public virtual void ApplyEffect() {

    }

    public virtual void Kill() {
        Destroy(gameObject);
    }

}
