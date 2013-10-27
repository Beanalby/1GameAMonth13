using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunDriver : MonoBehaviour {
    private static RunDriver _instance = null;
    public static RunDriver instance {
        get { return _instance; }
    }

    private const int ROOM_LENGTH = 10;
    private int nextRoomNum = 0;

    public GameObject roomPrefab, barrierPrefab, barrierWarningPrefab;

    GameObject currentRoom, nextRoom;

    public void Awake() {
        _instance = this;
    }

    public void Start() {
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
        barrierPos = new Vector3(Random.Range(-2, 2), 0, ROOM_LENGTH / 2);
        barrier = Instantiate(barrierPrefab) as GameObject;
        barrier.transform.parent = nextRoom.transform;
        barrier.transform.localPosition = barrierPos;
        warning = Instantiate(barrierWarningPrefab) as GameObject;
        warning.transform.parent = nextRoom.transform;
        warning.transform.localPosition = barrierPos;

        // and another barrier halfway through
        barrierPos = new Vector3(Random.Range(-2, 2), 0, 0);
        barrier = Instantiate(barrierPrefab) as GameObject;
        barrier.transform.parent = nextRoom.transform;
        barrier.transform.localPosition = barrierPos;
        warning = Instantiate(barrierWarningPrefab) as GameObject;
        warning.transform.parent = nextRoom.transform;
        warning.transform.localPosition = barrierPos;
        nextRoomNum++;

    }
}
