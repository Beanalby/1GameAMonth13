using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunDriver : MonoBehaviour {
    private static RunDriver _instance = null;
    public static RunDriver instance {
        get { return _instance; }
    }

    private const int ROOM_LENGTH = 10;
    private int currentRoomNum = 0;

    public GameObject roomPrefab;

    GameObject currentRoom, nextRoom;

    public void Awake() {
        _instance = this;
    }

    public void Start() {
        currentRoom = Instantiate(roomPrefab) as GameObject;
        nextRoom = Instantiate(roomPrefab) as GameObject;
        nextRoom.transform.position = new Vector3(0, 0, ROOM_LENGTH);
        
    }

    public void RoomEntered(GameObject room) {
        if(room != nextRoom) {
            // must be right at the start
            return;
        }
        // they're in the nextRoom.  It becomes the currentRoom, remove
        // the old currentRoom, and create a new nextRoom further ahead.
        Destroy(currentRoom);
        currentRoom = nextRoom;
        currentRoomNum++;
        nextRoom = Instantiate(roomPrefab) as GameObject;
        nextRoom.transform.position = new Vector3(0, 0, currentRoomNum * ROOM_LENGTH);
    }
}
