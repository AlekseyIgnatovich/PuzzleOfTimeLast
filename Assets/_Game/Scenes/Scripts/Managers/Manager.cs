using UnityEngine;

public abstract class Manager : MonoBehaviour {

    public GameManager gameManager;
    public Canvas canvas;

    public virtual void Initialize(GameManager _manager) {
        gameManager = _manager;
        canvas.worldCamera = Camera.main;
    }

    public virtual void Kill() {
        Destroy(gameObject);
    }
}
