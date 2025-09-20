using UnityEngine;
using System;

public class TogglerButton : MonoBehaviour {

    bool updating;
    public bool buttonSet;
    [Space]
    [SerializeField] Transform button;
    [SerializeField] Transform positionOn;
    [SerializeField] Transform positionOff;
    [SerializeField] GameObject setOn;
    [SerializeField] GameObject setOff;
    [Space]
    [SerializeField] float smoothTime;

    public event Action OnToggleChange;

    private void Update() {
        if (updating) {
            Vector2 _pos = button.position;
            Vector2 _newpos = positionOn.position;
            if (!buttonSet) { _newpos = positionOff.position; }

            if (_pos == _newpos) {
                updating = false;
            } else {
                Vector2 _spd = Vector2.zero;
                _pos = Vector2.SmoothDamp(_pos, _newpos, ref _spd, smoothTime);
            }
            button.position = _pos;
        }
    }

    public void SetToggle(bool _set, bool _update = false) {
        updating = _update;
        if (buttonSet != _set) { updating = true; }

        buttonSet = _set;
        setOn.SetActive(buttonSet);
        setOff.SetActive(!buttonSet);
    }
    public void Trigger() {
        OnToggleChange?.Invoke();
    }

}
