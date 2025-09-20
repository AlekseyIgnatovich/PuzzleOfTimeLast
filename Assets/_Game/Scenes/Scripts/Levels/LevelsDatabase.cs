using UnityEngine;
using System;

[CreateAssetMenu(menuName = "My File/Levels Database")]
public class LevelsDatabase : ScriptableObject
{

    public string mapName;

    public int progress;
    public int selectedLevel;

    public Level[] levels;

    public event Action OnSlectionChange;

    public void SelectLevel(int selection)
    {
        selectedLevel = Mathf.Clamp(selection, 0, levels.Length - 1);
        OnSlectionChange?.Invoke();
    }
    public Level GetLevel(int index)
    {
        return levels[index];
    }
    public Level GetSelectedLevel()
    {
        return levels[selectedLevel];
    }
    public void WonLevel()
    {
        progress = Mathf.Max(progress, selectedLevel + 1);
        GameManager.onLevelFinished?.Invoke(mapName, progress);
        if (selectedLevel + 1 >= levels.Length)
        {
            GameManager.onMapFinished?.Invoke(mapName);
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt($"progress {name}", progress);
        PlayerPrefs.Save();
    }
    public void Load(int _level)
    {
        //progress = PlayerPrefs.GetInt($"progress {name}", 0);
        progress = _level;
    }
}
