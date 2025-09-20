using UnityEngine;

[CreateAssetMenu(menuName = "My File/Heroes/Classes/Wizard")]
public class HeroClassWizard : HeroClass {

    public HeroClassWizard() {
        Name = "Wizard";
        description = "The Wizard can either deal damage to an enemy, or heal an ally.";
        color = Color.magenta;

        attackModifier = .4f;
        defenseModifier = .1f;
        healthModifier = 1f;
        specialModifier = .4f;
    }
}
