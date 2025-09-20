using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour {

    [SerializeField] GameplayManager manager;
    [SerializeField] GameObject body;
    [SerializeField] TextMeshProUGUI textMinutes;
    [SerializeField] TextMeshProUGUI textSeconds;
    [SerializeField] Transform setPoint;

    float timer;

    bool timerrunning;

    public float Timer {
        get { return timer; }
        set {
            timer = value;
            UpdateTime();
        }
    }

    private void Start() {
        transform.position = setPoint.position;
    }

    private void Update() {
        if (timerrunning) {
            Timer -= Time.deltaTime;
            if (Timer <= 0) {
                Setup(false);
                manager.level.cutsceneType = CutsceneType.CutsceneWin;
                manager.SetState(GameplayState.Cutscene);
                print("Timer finished");
            }
        } else {
            if (Timer > 0) {
                Setup(true);
                print("Timer turned on");
            }
        }
    }

    void UpdateTime() {
        float _mins = Mathf.Floor(timer / 60);
        textMinutes.text = $"{_mins}";
        textSeconds.text = $"{Mathf.Floor(timer - (_mins * 60))}";
    }
    public void Setup(bool _set) {
        timerrunning = _set;
        body.SetActive(_set);
    }
}
