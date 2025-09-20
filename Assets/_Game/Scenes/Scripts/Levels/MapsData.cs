using UnityEngine;

[CreateAssetMenu(menuName = "My File/Maps Database")]
public class MapsData : ScriptableObject {

    public int progress;
    public int selectedMap;
    [Space]
    public Manager[] maps;
    public LevelsDatabase[] levelDatas;

    public void SelectMap(int selection) {
        selectedMap = Mathf.Clamp(selection, 0, maps.Length - 1);
    }
    public Manager GetMap(int index) {
        return maps[index];
    }
    public Manager GetSelectedMap() {
        return maps[selectedMap];
    }

    public string GetSelectedMapName() {
        return levelDatas[selectedMap].mapName;
    }
}
