using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class Letter : MonoBehaviour {

    [HideInInspector]
    public bool invincible = true;

    private float deathFlingStrength = 10f;
    private float deathSpinStrength = 20f;
    private float deathDuration = 1f;

    public IEnumerator GotHit(GameObject bullet) {
        GetComponent<ParticleSystem>().Play();
         // don't cause collisions with anything else as we fling
        GetComponent<Collider>().enabled = false;
        // clear our parent Wave so we aren't "pulled forward" anymore
        transform.parent = null;
        // throw ourselves backward based on the bullet's forward
        Vector3 dir = bullet.transform.forward;
        dir.y = Random.Range(.3f, .6f);
        dir *= deathFlingStrength;
        rigidbody.velocity = dir;
        rigidbody.angularVelocity = new Vector3(
            Random.Range(-deathSpinStrength, deathSpinStrength),
            Random.Range(-deathSpinStrength, deathSpinStrength),
            Random.Range(-deathSpinStrength, deathSpinStrength));
        rigidbody.useGravity = true;
        yield return new WaitForSeconds(deathDuration);
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
        if(!invincible) {
            StartCoroutine(GotHit(other.gameObject));
        }
    }
}