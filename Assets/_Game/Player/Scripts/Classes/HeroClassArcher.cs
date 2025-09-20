using UnityEngine;

[CreateAssetMenu(menuName = "My File/Heroes/Classes/Archer")]
public class HeroClassArcher : HeroClass {

    public HeroClassArcher() {
        Name = "Archer";
        description = "The Archer can deal high damage to a selected target.";
        color = Color.yellow;

        attackModifier = .5f;
        defenseModifier = .1f;
        healthModifier = 1f;
        specialModifier = .1f;
    }
}
