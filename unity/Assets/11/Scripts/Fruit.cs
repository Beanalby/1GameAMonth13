using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Fruit : MonoBehaviour {

    private bool isCaught = false;
    public bool IsCaught {
        get { return isCaught; }
    }

    public void Caught() {
        rigidbody.isKinematic = true;
        isCaught = true;
    }
    public void Released(Vector3 velocity) {
        transform.parent = null;
        Rigidbody rb = rigidbody;
        rb.isKinematic = false;
        rb.velocity = velocity;
        isCaught = false;
    }
}
