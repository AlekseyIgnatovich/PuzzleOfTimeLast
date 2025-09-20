using UnityEngine;
using TMPro;

public class TurnPopup : MonoBehaviour {

    [SerializeField] TextMeshProUGUI text;

    float timer;

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0) {
            GetComponent<Animator>().SetTrigger("end");
        }
    }

    public void Init(string _text, float _time) {
        text.text = _text;
        timer = _time;
    }

    public void Kill() {
        Destroy(gameObject);
    }

}
