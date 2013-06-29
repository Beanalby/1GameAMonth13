using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float damage = 50f;

    public void OnTriggerEnter(Collider other) {
        Debug.Log("Sending takeDamage to " + other.gameObject.name);
        other.gameObject.SendMessage("TakeDamage", this,
            SendMessageOptions.DontRequireReceiver);
    }
}

