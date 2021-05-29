using UnityEngine;
using System.Collections;

public class CameraSwapX : MonoBehaviour {
    public GameObject leftCamera;
    public GameObject rightCamera;

    public void OnTriggerEnter(Collider other) {
        // only swap for the player
        Player p = other.GetComponent<Player>();
        if(p == null || p.IsDead) {
            return;
        }
        if(p.GetComponent<Rigidbody>()==null) {
            Debug.LogError("!!! No rigidbody on player!");
            return;
        }
        // switch camera based on which direction they're heading
        if(other.GetComponent<Rigidbody>().velocity.x >= 0) {
            CameraManager.instance.Current = rightCamera;
        } else {
            CameraManager.instance.Current = leftCamera;
        }
    }
}
