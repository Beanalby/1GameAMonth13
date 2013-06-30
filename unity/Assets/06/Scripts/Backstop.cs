using UnityEngine;
using System.Collections;

public class Backstop : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
    }

    public void TakeDamage(Damage damage) {
        // psch. whatever.  Bring it.
    }
}
