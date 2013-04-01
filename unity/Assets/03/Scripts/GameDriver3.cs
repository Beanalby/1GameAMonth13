using UnityEngine;
using System.Collections;

public enum GameDriver3State { Starting, Running, Finished };

public class GameDriver3 : MonoBehaviour {

    public GUISkin skin;
    public AudioClip countdownSound;
    public AudioClip countdownFinishedSound;

    private GameDriver3State state = GameDriver3State.Starting;
    Cart cart;
    ballLauncher launcher;
    private float startTime;

    private int countdownLastNum = -1;

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
        startTime = Time.time;
        cart = GameObject.Find("Cart").GetComponent<Cart>();
        launcher = cart.GetComponentInChildren<ballLauncher>();

    }
    public void OnGUI() {
        GUI.skin = skin;
        GUI.Label(new Rect(0, 70, Screen.width, Screen.height), "Score: " + score);
        switch(state) {
            case GameDriver3State.Starting:
                int timeLeft = 3 - (int)(Time.time - startTime);
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 100), timeLeft.ToString());
                if(timeLeft == 0) {
                    state = GameDriver3State.Running;
                    launcher.isRunning = true;
                    cart.StartMove(cart.startingTrack);
                    AudioSource.PlayClipAtPoint(countdownFinishedSound, Camera.current.transform.position);
                } else if(timeLeft != countdownLastNum) {
                    countdownLastNum = timeLeft;
                    AudioSource.PlayClipAtPoint(countdownSound, Camera.current.transform.position);
                }
                break;
            case GameDriver3State.Finished:
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 100), "Finished!");
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 100), "Score: " + score);
                if(GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 200, 200, 75), "Try Again")) {
                    Application.LoadLevel(Application.loadedLevel);
                }
                if(GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 200, 200, 75), "Choose Stage")) {
                    Application.LoadLevel("stageSelect");
                }
                break;
        }
    }
    public void TrackFinished() {
        state = GameDriver3State.Finished;
    }
    public void ShotSuccess(Rim rim) {
        score += rim.value;
    }
}
