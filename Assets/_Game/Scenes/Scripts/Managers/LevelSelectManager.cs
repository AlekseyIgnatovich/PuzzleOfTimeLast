using UnityEngine;

public class LevelSelectManager : Manager {

    public void StartGame() {
        gameManager.SetState(GameState.Gameplay);
    }
}
