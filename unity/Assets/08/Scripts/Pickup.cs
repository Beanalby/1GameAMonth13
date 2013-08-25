using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
    
    public void OnTriggerEnter(Collider other) {
        GameDriver8.instance.AddPickup(name);
        Destroy(gameObject);
    }
}
