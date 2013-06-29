using UnityEngine;
using System.Collections;

public class Backstop : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
    }
}
