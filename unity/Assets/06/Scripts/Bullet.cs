using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float damage = 50f;

    public void OnTriggerEnter(Collider other) {
        other.gameObject.SendMessage("TakeDamage", this,
            SendMessageOptions.DontRequireReceiver);
    }
}

