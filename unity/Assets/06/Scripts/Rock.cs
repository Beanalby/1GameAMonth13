using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

    public AudioClip rockGotHit;
    public GameObject rockHitEffect;

    [HideInInspector]
    public Transform target;

    private int damage = 25;
    [HideInInspector]
    public float speed=10f;

    private Vector3 dir;
    private Transform plane;
    private Transform flame;

    public void Start() {
        flame = transform.Find("Flame");
        plane = transform.Find("Plane");
        dir = (target.transform.position - transform.position).normalized;
        flame.rotation = Quaternion.LookRotation(-dir);
    }
    public void Update() {
        Vector3 dir = Camera.main.transform.position - plane.transform.position;
        plane.transform.rotation = Quaternion.LookRotation(dir);
    }
    public void FixedUpdate() {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other) {
        other.gameObject.SendMessage("TakeDamage",
            new Damage(this.transform, damage),
            SendMessageOptions.RequireReceiver);
        Destroy(gameObject);
    }

    public void TakeDamage(Damage damage) {
        // we hit a bullet / a bullet hit us, both go away
        AudioSource.PlayClipAtPoint(rockGotHit, Camera.main.transform.position);
        Instantiate(rockHitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
