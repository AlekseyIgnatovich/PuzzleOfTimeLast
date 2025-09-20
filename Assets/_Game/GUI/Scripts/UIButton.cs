using UnityEngine;
using UnityEngine.Events;

public class UIButton : MonoBehaviour {

    public UnityEvent buttonExecute;

    public virtual void Execute() {
        buttonExecute?.Invoke();
    }

}
