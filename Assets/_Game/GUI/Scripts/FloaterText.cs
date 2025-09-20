using UnityEngine;
using TMPro;

public class FloaterText : MonoBehaviour {

    [SerializeField] TextMeshProUGUI text;

    public void Setup(string _text, float _size, Color _color) {
        text.text = _text;
        text.fontSize = _size;
        text.color = _color;

        print($"Created a floater [{_text}]");
    }

    public void Kill() {
        Destroy(gameObject);
    }
}
