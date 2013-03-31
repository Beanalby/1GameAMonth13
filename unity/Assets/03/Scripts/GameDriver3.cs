using UnityEngine;
using System.Collections;

public enum GameDriver3State { Running, Finished };

public class GameDriver3 : MonoBehaviour {

    public GUISkin skin;

    private GameDriver3State state = GameDriver3State.Running;

    public GameDriver3State State {
        get { return state; }
    }

    private int score = 0;
    public void Start() {
        ShowControls controls = ShowControls.CreateDocked(new ControlItem[] {
            new ControlItem("Use the mouse to aim, Left Click to shoot balls", MouseDirection.Both, MouseButton.LeftClick),
            new ControlItem("Use W/S to increase/decrease power", new KeyCode[] { KeyCode.W, KeyCode.S })
        });
        controls.showDuration = -1;
        controls.Show();
    }
    public void OnGUI() {
        GUI.skin = skin;
        GUI.Label(new Rect(0, 70, Screen.width, Screen.height), "Score: " + score);
        if(state == GameDriver3State.Finished) {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 100), "Finished!");
        }
    }
    public void TrackFinished() {
        state = GameDriver3State.Finished;
    }
    public void ShotSuccess(Rim rim) {
        score += rim.value;
    }
}
