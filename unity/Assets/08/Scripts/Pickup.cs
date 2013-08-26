using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    private float spinSpeed = 30f;

    private Transform mesh;
    public void Start() {
        mesh = transform.Find("pickupMesh");
    }
    public void Update() {
        mesh.transform.rotation = Quaternion.Euler(
            new Vector3(0, Time.time * spinSpeed, 0));
    }
    public void OnTriggerEnter(Collider other) {
        GameDriver8.instance.AddPickup(name);
        Destroy(gameObject);
    }
}
