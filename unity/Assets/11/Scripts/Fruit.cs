using UnityEngine;
using System.Collections;

public enum FruitType { Apple, Lime, Orange };

public class Fruit : MonoBehaviour {

    public FruitType Type;

    private int groundLayer;

    private bool hasBeenCaught = false;
    public bool HasBeenCaught {
        get { return hasBeenCaught; }
    }

    public void Start() {
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    public void OnCollisionEnter(Collision collision) {
        // we only react with ground collisions
        if(collision.transform.gameObject.layer != groundLayer) {
            return;
        }
        Debug.Log("Fruit going SPLAT at " + collision.contacts[0].point);
        Destroy(gameObject);
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
