using UnityEngine;

public class HeroHighlight : MonoBehaviour {

    public void Setup(ElementType _type, Vector3 _pos) {
        gameObject.SetActive(true);

        transform.position = _pos;

        int c = (int)_type;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(c == i);
        }
    }

}
