using UnityEngine;

public class MenuButton : MonoBehaviour {

    [SerializeField] Animator animator;

    public void SetButton(bool _set) {
        animator.SetBool("active", _set);
    }

}
