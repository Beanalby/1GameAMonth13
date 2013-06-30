using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public int damage = 10;

    public void OnTriggerEnter(Collider other) {
        other.gameObject.SendMessage("TakeDamage",
            new Damage(this.transform, damage),
            SendMessageOptions.RequireReceiver);
        Destroy(gameObject);
    }

    public void TakeDamage(Damage damage) {
        // we hit a rock / a rock hit us, both go away
        Destroy(gameObject);
    }
}