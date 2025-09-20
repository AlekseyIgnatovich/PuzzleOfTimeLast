using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName ="My File/System/Game Settings")]
public class GameSettings : ScriptableObject {

    public int saveVersion;
    [Space]
    public float[] rarityXP;
    public ExperienceLevels xpThreshold;
    public int[] maxLevels;


#if UNITY_EDITOR
    [MenuItem("KNOX/Asset Modding/Set Levels Resistances")]
    private static void MenuItem() {
        Debug.Log("Set Resistances of all Levels to 2");

        string[] _resault = AssetDatabase.FindAssets("Level", new[] { "Assets/Scenes/Levels" });

        string _path;
        Level _lvl;
        for (int i = 0; i < _resault.Length; i++) {
            _path = AssetDatabase.GUIDToAssetPath(_resault[i]);

            _lvl = AssetDatabase.LoadAssetAtPath<Level>(_path);

            //if (_obj != null) {
            //Debug.Log($"We have an Object folks [{_obj.name}]");

            //_lvl = _obj.GetComponent<Level>();

            if (_lvl != null) {
                _lvl.SetAllResistances(2);
            }
            //}
        }
    }

    [MenuItem("KNOX/Miscelenious/Delete PlayerPrefs")]
    private static void MenuItem1() {
        Debug.Log("Deleted PlayerPrefs");
        PlayerPrefs.DeleteAll();
    }
#endif
}
