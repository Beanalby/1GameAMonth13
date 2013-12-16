using UnityEngine;
using System.Collections;

public enum CuteStageState { intro, playing, success, fail };

public class CuteStageDriver : MonoBehaviour {
    public GUISkin skin;

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

    public void Awake() {
        if(instance != null) {
            Debug.LogError("CuteStageDriver already exists");
            Destroy(gameObject);
        }
        instance = this;
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
        Rect stateRect = new Rect(0, 0, Screen.width, 200);
        string stateLabel = "";
        switch(state) {
            case CuteStageState.success:
                stateLabel = "Success!";
                break;
            case CuteStageState.fail:
                stateLabel = "Failure :(";
                break;
        }
        if(stateLabel != "") {
            GUI.Label(stateRect, stateLabel);
        }
    }
}
