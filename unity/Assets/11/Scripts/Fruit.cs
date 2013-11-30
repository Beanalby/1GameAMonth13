using UnityEngine;
using System.Collections;

public enum FruitType { Apple, Lime, Orange };

public class Fruit : MonoBehaviour {

    public FruitType Type;
    public GameObject splatPrefab;

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
        Vector3 pos = collision.contacts[0].point;

        // if the collision point is below zero vertically, then this was a
        // fast horizontal fruit that went into a bin, but then hit
        // a side "wall" of the floor.  Don't splat if that's the case.
        if(pos.y < -0.001f) {
            Destroy(gameObject);
            return;
        }

        SplatEffect splat = (Instantiate(splatPrefab) as GameObject).GetComponent<SplatEffect>();
        splat.Type = Type;
        splat.transform.position = new Vector3(pos.x, .01f, 0);

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
