using UnityEngine;

[CreateAssetMenu(menuName = "My File/Mobs/Mob Base")]
public class MobBase : ScriptableObject {

    [Header("List with all the mobs")]
    public MobCard[] mob;

    [Header("Mobs to spawn")]
    public MobCard leftMob;
    public MobCard middleMob;
    public MobCard rightMob;

}
