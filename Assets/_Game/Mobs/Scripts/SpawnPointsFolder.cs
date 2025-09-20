using UnityEngine;

public class SpawnPointsFolder : MonoBehaviour {

    [SerializeField] SpawnPoint[] spawnPoints1;
    [SerializeField] SpawnPoint[] spawnPoints2;
    [SerializeField] SpawnPoint[] spawnPoints3;
    [SerializeField] SpawnPoint[] spawnPoints4;
    [SerializeField] SpawnPoint[] spawnPoints5;
    [Space]
    [SerializeField] GameObject[] mobTargets1;
    [SerializeField] GameObject[] mobTargets2;
    [SerializeField] GameObject[] mobTargets3;
    [SerializeField] GameObject[] mobTargets4;
    [SerializeField] GameObject[] mobTargets5;
    [Space]
    [SerializeField] GameObject[] healthBars;


    public SpawnPoint[] GetSpawnPoints(int _length) {
        switch (_length) {
            case 1:
                return spawnPoints1;
            case 2:
                return spawnPoints2;
            case 3:
                return spawnPoints3;
            case 4:
                return spawnPoints4;
            case 5:
                return spawnPoints5;
        }

        return null;
    }

    public GameObject[] GetMobTargets(int _length) {
        switch (_length) {
            case 1:
                return mobTargets1;
            case 2:
                return mobTargets2;
            case 3:
                return mobTargets3;
            case 4:
                return mobTargets4;
            case 5:
                return mobTargets5;
        }

        return null;
    }

    public void CloseHealthBars(int _length) {
        for (int i = 0; i < healthBars.Length; i++) {
            healthBars[i].SetActive(false);
        }
    }
}
