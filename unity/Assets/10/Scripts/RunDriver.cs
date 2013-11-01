using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunDriver : MonoBehaviour {

    public GUISkin skin;

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
        runnerInfo = GameObject.Find("RunnerInfo").GetComponent<RunnerInfo>();
        runnerInfo.SpawnRunner();

        currentRoom = Instantiate(roomPrefab) as GameObject;
        currentRoom.name = "room0";
        nextRoom = Instantiate(roomPrefab) as GameObject;
        nextRoom.name = "room1";
        nextRoomNum = 2;
        nextRoom.transform.position = new Vector3(0, 0, ROOM_LENGTH);
        
    }

    public void RoomEntered(GameObject room) {
        // they're in the nextRoom.  It becomes the currentRoom, remove
        // the old currentRoom, and create a new nextRoom further ahead.
        Debug.Log("Destroying current [" + currentRoom.name
            + "], Creating new room #" + nextRoomNum);
        Destroy(currentRoom);
        currentRoom = nextRoom;
        nextRoom = Instantiate(roomPrefab) as GameObject;
        nextRoom.name = "room" + nextRoomNum;
        nextRoom.transform.position = new Vector3(0, 0, nextRoomNum * ROOM_LENGTH);

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
        nextRoomNum++;

    }

    public void OnGUI() {
        GUI.skin = skin;
        GUI.Label(new Rect(10, 10, 200, 20),
            "Speed: " + runnerInfo.Speed.ToString("0.00"));
        GUI.Label(new Rect(10, 30, 200, 100),
            "Distance: " + runnerInfo.DistanceTravelled.ToString("0.0"));
        GUI.Label(new Rect(Screen.width - 210, 10, 200, 20),
            "Health: " + runnerInfo.Health.ToString("0."), healthStyle);
        if(runnerInfo.Health <= 0) {
            Rect buttonRect = new Rect(Screen.width/2 - 100, Screen.height/2 - 130,
                200, 60);
            if(GUI.Button(buttonRect, "Play Again")) {
                Application.LoadLevel(Application.loadedLevel);
            }
            }
    }
}
