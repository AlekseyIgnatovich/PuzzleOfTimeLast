using UnityEngine;

public class LinkedEnabling : MonoBehaviour {

    [SerializeField] GameObject[] links;

    private void OnEnable() {
        SetLinks(true);
    }
    private void OnDisable() {
        SetLinks(false);
    }

    void SetLinks(bool _set) {
        for (int i = 0; i < links.Length; i++) {
            links[i].SetActive(_set);
        }
    }
}
