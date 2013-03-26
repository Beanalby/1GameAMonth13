using UnityEngine;
using System.Collections;

public class GameDriver3 : MonoBehaviour {

    public GUISkin skin;

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
    }
    public void ShotSuccess(Rim rim) {
        score += rim.value;
    }
}
