using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MobTypeAutomatization : MonoBehaviour
{
    public Level[] allLevels;
    public RewardItem[] heroCardRewards;
    public MobCard[] allEnemies;


    private void Start()
    {
        SetEnemiesTypeFromScriptable();
        // for (int i = 0; i < allLevels.Length; i++)
        // {
        //     Debug.Log("Seteando Level: " + allLevels[i].name);
        //     SetResistancesFromLevel(i);
        // }
    }


    void SetResistancesFromLevel(int _index)
    {
        for (int j = 0; j < allLevels[_index].waves.Length; j++)
        {
            Debug.Log("Wave: " + j);
            foreach (var enem in allLevels[_index].waves[j].enemy)
            {
                Debug.Log("Seteando enemigo: " + enem.mob.name + " - type: " + enem.mob.type);
                if (enem.mob.isCustom || enem.mob.type != ElementType.None) continue;
                //enem.mob.type = enem.type;
                enem.mob.type = GenerateRandomType();
                enem.mob.resistances = new Resistance[2];
                enem.mob.resistances = GameManager.instance.GenerateDefaultResistances(enem.mob.type);
#if UNITY_EDITOR
                EditorUtility.SetDirty(enem.mob);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif
            }
        }
    }

    void SetEnemiesTypeFromScriptable()
    {
        foreach (var mob in allEnemies)
        {
            mob.resistances = new Resistance[2];
            mob.resistances = GameManager.instance.GenerateDefaultResistances(mob.type);
            mob.type = GenerateRandomType();
#if UNITY_EDITOR
            EditorUtility.SetDirty(mob);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }
    }
    public ElementType GenerateRandomType()
    {
        ElementType[] values = (ElementType[])System.Enum.GetValues(typeof(ElementType));

        int randomIndex = Random.Range(1, values.Length);
        return values[randomIndex];
    }
}