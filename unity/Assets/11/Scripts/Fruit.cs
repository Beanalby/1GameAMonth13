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
        // tweak the trailrenderer's color based on our type
        Material mat = GetComponentInChildren<TrailRenderer>().material;
        mat.SetColor("_Emission", SplatEffect.type2color[Type]);
    }

    public void OnCollisionEnter(Collision collision) {
        // we only react with ground collisions
        if(collision.transform.gameObject.layer != groundLayer) {
            return;
        }
        Vector3 pos = collision.contacts[0].point;

        GameObject.Find("FruitGameDriver").GetComponent<FruitGameDriver>().FruitSplat();
        SplatEffect splat = (Instantiate(splatPrefab) as GameObject).GetComponent<SplatEffect>();
        splat.Type = Type;
        splat.transform.position = new Vector3(pos.x, .01f, 0);

        Destroy(gameObject);
    }

    public void Init(Vector3 position, Vector3 velocity, Vector3 spin) {
        transform.position = position;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;
        rb.angularVelocity = spin;
    }

    public void Caught() {
        GetComponent<Rigidbody>().isKinematic = true;
        hasBeenCaught = true;
        GetComponentInChildren<TrailRenderer>().enabled = false;
    }
    public void Released(Vector3 velocity) {
        transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = velocity;
    }
}
