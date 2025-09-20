using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour {

    [SerializeField] Animator animator;
    [Space]
    [SerializeField] Image box;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] GameObject symbol;

    int activated = 0;

    public void Setup(string _text) {
        message.text = _text;
        transform.localPosition = Vector3.zero;
    }

    public void TriggerNotification() {
        switch (activated) {
            case 0:
                animator.SetTrigger("show");
                break;
            case 1:
                animator.SetTrigger("hide");
                break;
            default:
                Kill();
                break;
        }
        activated++;
    }

    public void Kill() {
        Destroy(gameObject);
    }
}
