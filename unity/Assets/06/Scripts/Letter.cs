using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {

    public void GotHit() {
        Debug.Log("[" + name + " is dead!");
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
        Destroy(other.gameObject);
    }
}