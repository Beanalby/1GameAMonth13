using UnityEngine;
using System.Collections;

public class FruitBin : MonoBehaviour {
    public void OnTriggerEnter(Collider col) {
        Debug.Log(name + " got " + col.name);
    }
}
