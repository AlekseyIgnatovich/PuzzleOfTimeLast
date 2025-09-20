using UnityEngine;
using UnityEngine.UI;

public class DialogueParty : MonoBehaviour {

    [SerializeField] Animator animator;
    public Image picture;

    public void Setup(DialogueLine _line) {
        picture.sprite = _line.actorSprite;
    }
    public void End() {
        animator.SetTrigger("end");
    }

    public void Kill() {
        Destroy(gameObject);
    }
}
