using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RunnerGameState { Intro, Starting, Running };

public class RunDriver : MonoBehaviour {

    public GUISkin skin;

    private RunnerGameState state;
    public RunnerGameState State {
        get { return state; }
    }

    private static RunDriver _instance = null;
    public static RunDriver instance {
        get { return _instance; }
    }

    private const int ROOM_LENGTH = 10;
    private int nextRoomNum = 0;

    public GameObject roomPrefab, barrierPrefab, barrierWarningPrefab;

    GameObject currentRoom, nextRoom;

    private RunnerInfo runnerInfo;
    private GUIStyle healthStyle;

    public void Awake() {
        _instance = this;
        healthStyle = new GUIStyle(skin.label);
        healthStyle.alignment = TextAnchor.UpperRight;
    }

    public void Start() {
        state = RunnerGameState.Intro;
        runnerInfo = GameObject.Find("RunnerInfo").GetComponent<RunnerInfo>();
        runnerInfo.SpawnRunner();

        currentRoom = Instantiate(roomPrefab) as GameObject;
        currentRoom.name = "room0";
        nextRoom = Instantiate(roomPrefab) as GameObject;
        nextRoom.name = "room1";
        nextRoomNum = 2;
        nextRoom.transform.position = new Vector3(0, 0, ROOM_LENGTH);
        
    }

    public void Update() {
        if(state == RunnerGameState.Intro && Input.GetKeyDown(KeyCode.Space)) {
            state = RunnerGameState.Starting;
            runnerInfo.EnableControl();
        }
        if(runnerInfo.Health <= 0 && Input.GetKeyDown(KeyCode.Space)) {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void RoomEntered(GameObject room) {
        // they're in the nextRoom.  It becomes the currentRoom, remove
        // the old currentRoom, and create a new nextRoom further ahead.
        Destroy(currentRoom);
        currentRoom = nextRoom;
        nextRoom = Instantiate(roomPrefab) as GameObject;
        nextRoom.name = "room" + nextRoomNum;
        nextRoom.transform.position = new Vector3(0, 0, nextRoomNum * ROOM_LENGTH);

        switch(state) {
            case RunnerGameState.Intro:
                // nothing appears in the rooms
                break;
            case RunnerGameState.Starting:
                state = RunnerGameState.Running;
                break;

            case RunnerGameState.Running:
                Vector3 barrierPos;
                GameObject barrier, warning;
                // make a barrier at the start of the room
                barrierPos = new Vector3(Random.Range(-2, 2), 0,
                    ROOM_LENGTH / 2 + Random.Range(-1f, 1f));
                barrier = Instantiate(barrierPrefab) as GameObject;
                barrier.transform.parent = nextRoom.transform;
                barrier.transform.localPosition = barrierPos;
                warning = Instantiate(barrierWarningPrefab) as GameObject;
                warning.transform.parent = nextRoom.transform;
                warning.transform.localPosition = barrierPos;

                // and another barrier halfway through
                barrierPos = new Vector3(Random.Range(-2, 2), 0, Random.Range(-1f, 1f));
                barrier = Instantiate(barrierPrefab) as GameObject;
                barrier.transform.parent = nextRoom.transform;
                barrier.transform.localPosition = barrierPos;
                warning = Instantiate(barrierWarningPrefab) as GameObject;
                warning.transform.parent = nextRoom.transform;
                warning.transform.localPosition = barrierPos;
                break;
        }
        nextRoomNum++;
    }

    public void AttachToNextRoom(Transform t) {
        t.parent = nextRoom.transform;
    }

    public void OnGUI() {
        GUI.skin = skin;
        GUI.Label(new Rect(10, 10, 200, 20),
            "Speed: " + runnerInfo.Speed.ToString("0.00"));
        GUI.Label(new Rect(10, 30, 200, 100),
            "Distance: " + runnerInfo.DistanceTravelled.ToString("0.0"));
        GUI.Label(new Rect(Screen.width - 210, 10, 200, 20),
            "Health: " + runnerInfo.Health.ToString("0."), healthStyle);
        
        Rect centerRect = new Rect(Screen.width/2 - 100, Screen.height/2 - 130,
            200, 60);

        if(runnerInfo.Health <= 0) {
            GUI.Label(new Rect(0, Screen.height / 2 - 200, Screen.width, 100),
                "Travelled " + runnerInfo.DistanceTravelled.ToString("0.0") + "m",
                skin.customStyles[0]);
            if(GUI.Button(centerRect, "Play Again")) {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
