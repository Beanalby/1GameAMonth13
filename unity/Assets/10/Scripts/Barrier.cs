using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour {

    int playerLayer;
    public GameObject crashEffect;
    private int damage = 1;
    public AudioClip crashSound;

    public void Awake() {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void GotHit() {
        Instantiate(crashEffect, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(crashSound, transform.position);
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer != playerLayer) {
            return;
        }
        collision.gameObject.SendMessage("Crashed", damage);
        GotHit();
    }
}
