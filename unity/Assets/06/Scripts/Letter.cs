using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {

    public void GotHit() {
        Debug.Log("[" + name + " is dead!");
        Destroy(gameObject);
    }
}