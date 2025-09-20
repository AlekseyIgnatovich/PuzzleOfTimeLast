using UnityEngine;

[System.Serializable]
public class ExperienceLevels {

    public float[] roof;

    public float GetXPThreshold(int _level) {
        if (_level < 0) { return 0; }
        return roof[Mathf.Clamp(_level, 0, roof.Length - 1)];
    }
}
