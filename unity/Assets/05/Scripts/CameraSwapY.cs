using UnityEngine;
using System.Collections;

public class CameraSwapY : MonoBehaviour {
    public GameObject topCamera;
    public GameObject bottomCamera;

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
        if(other.GetComponent<Rigidbody>().velocity.y >= 0) {
            CameraManager.instance.Current = topCamera;
        } else {
            CameraManager.instance.Current = bottomCamera;
        }
    }
}
