using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Fruit : MonoBehaviour {

    private bool hasBeenCaught = false;
    public bool HasBeenCaught {
        get { return hasBeenCaught; }
    }

    public void Init(Vector3 position, Vector3 velocity, Vector3 spin) {
        transform.position = position;
        Rigidbody rb = rigidbody;
        rb.velocity = velocity;
        rb.angularVelocity = spin;
    }

    public void Caught() {
        rigidbody.isKinematic = true;
        hasBeenCaught = true;
    }
    public void Released(Vector3 velocity) {
        transform.parent = null;
        Rigidbody rb = rigidbody;
        rb.isKinematic = false;
        rb.velocity = velocity;
    }
}
