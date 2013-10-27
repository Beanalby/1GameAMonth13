using UnityEngine;
using System.Collections;

public class RoomStart : MonoBehaviour {
    int playerLayer;

    public void Awake() {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == playerLayer) {
            RunDriver.instance.SendMessage("RoomEntered", transform.parent.gameObject);
        }
    }
}
