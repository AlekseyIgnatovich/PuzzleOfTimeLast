using UnityEngine;

[CreateAssetMenu(menuName = "My File/Heroes/Classes/Ranger")]
public class HeroClassRanger : HeroClass {

    public HeroClassRanger() {
        Name = "Ranger";
        description = "The Ranger can deal damage to all enemies.";
        color = Color.red;

        attackModifier = .3f;
        defenseModifier = .1f;
        healthModifier = 2f;
        specialModifier = .3f;
    }
}
