using UnityEngine;

[CreateAssetMenu(menuName ="My File/Heroes/Classes/Guardian")]
public class HeroClassGuardian : HeroClass {

    public HeroClassGuardian() {
        Name = "Guardian";
        description = "The Guardian`s special attack can strike the adjecent enemies to the target and either buff allies or nerf enemies, chosen at random.";
        color = Color.cyan;

        attackModifier = .2f;
        defenseModifier = .5f;
        healthModifier = 3f;
        specialModifier = .1f;
    }
}
