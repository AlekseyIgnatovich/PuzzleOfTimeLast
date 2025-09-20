using UnityEngine;

public class LoadoutPopup : MonoBehaviour {

    public void Setup() {
        gameObject.SetActive(true);
    }
    public void Close() {
        gameObject.SetActive(false);
    }

}
