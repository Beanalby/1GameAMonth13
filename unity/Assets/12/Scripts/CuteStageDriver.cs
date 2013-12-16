using UnityEngine;
using System.Collections;

public enum CuteStageState { intro, playing, success, fail };

public class CuteStageDriver : MonoBehaviour {
    public string nextStage;
    public GUISkin skin;
    public string instructions;
    public bool showControls;

    private static CuteStageDriver instance;
    public static CuteStageDriver Instance {
        get {
            if(instance == null) {
                Debug.LogError("No CuteStageDriver established");
            }
            return instance;
        }
    }

    private CuteStageState state = CuteStageState.playing;
    public CuteStageState State {
        get { return state; }
    }

    public bool IsPlaying {
        get { return state == CuteStageState.playing; }
    }

    private ShowControls sc;

    public void Awake() {
        if(instance != null) {
            Debug.LogError("CuteStageDriver already exists");
            Destroy(gameObject);
        }
        instance = this;
    }
    public void Start() {
        CuteMusicPlayer.Instance.PlayMusic("stageMusic");
        string moveDesc = "Use the arrow keys to move the player around.";
        string attackDesc = "Hold the spacebar to aim an Super Headbutt.  Hold a direction to headbutt, and release the spacebar to let it fly!";
        if(showControls) {
            sc = ShowControls.CreateDocked(new ControlItem[] {
                new ControlItem(moveDesc, CustomDisplay.arrows),
                new ControlItem(attackDesc, KeyCode.Space)
            });
            sc.slideSpeed = -1;
            sc.showDuration = -1;
            sc.Show();
        }
    }
    public void OnGUI() {
        GUI.skin = skin;
        DrawStateUI();
    }

    private void FailStage() {
        state = CuteStageState.fail;
    }
    private void SucceedStage() {
        state = CuteStageState.success;
    }
    public void FlingFinished() {
        // if there are any attackable things left that most be destroyed,
        // the stage is failed.
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Attackable");
        foreach(GameObject obj in objs) {
            CuteAttackable attackable = obj.GetComponent<CuteAttackable>();
            if(attackable.MustDestroy) {
                FailStage();
                return;
            }
        }
        SucceedStage();
    }

    private void DrawStateUI() {
        Rect stateRect = new Rect(0, 70, Screen.width, 70);
        Rect buttonRect = new Rect(Screen.width/2 - 100, 140, 200, 35);
        switch(state) {
            case CuteStageState.playing:
                GUI.Label(stateRect, instructions, skin.customStyles[0]);
                break;
            case CuteStageState.success:
                GUI.Label(stateRect, "Success!");
                if(GUI.Button(buttonRect, "Continue") || Input.GetButtonDown("Fire1")) {
                    Application.LoadLevel(nextStage);
                }
                break;
            case CuteStageState.fail:
                GUI.Label(stateRect, "Failed! :(");
                if(GUI.Button(buttonRect, "Retry") || Input.GetButtonDown("Fire1")) {
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
        }
    }
}
