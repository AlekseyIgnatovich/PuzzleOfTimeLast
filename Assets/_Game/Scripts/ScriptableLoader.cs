using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ScriptableLoader : MonoBehaviour
{
    public EffectBoxReward[] itemEffects;
    public EffectBoxReward greyEffect;

    private void Start()
    {
        int _amount = 0;
        foreach (var item in itemEffects)
        {
            _amount += item.reward.Length;
        }
        greyEffect.reward = new LootboxReward[_amount];
        int _globalIndex = 0;
        for (int i = 0; i < itemEffects.Length; i++)
        {
            for (int j = 0; j < itemEffects[i].reward.Length; j++)
            {
                greyEffect.reward[_globalIndex] = itemEffects[i].reward[j];
                _globalIndex++;
            }
        }
#if UNITY_EDITOR
        EditorUtility.SetDirty(greyEffect);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}
