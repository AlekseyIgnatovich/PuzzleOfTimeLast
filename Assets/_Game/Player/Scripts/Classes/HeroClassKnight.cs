using UnityEngine;

[CreateAssetMenu(menuName = "My File/Heroes/Classes/Knight")]
public class HeroClassKnight : HeroClass {

    public HeroClassKnight() {
        Name = "Knight";
        description = "The Knight can strike all adjecent enemies to his target.";
        color = Color.blue;

        attackModifier = .3f;
        defenseModifier = .3f;
        healthModifier = 3.5f;
        specialModifier = .2f;
    }
}
