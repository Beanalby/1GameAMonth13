using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class Letter : MonoBehaviour {

    public float MaxHealth = 100;
    public Material HitFlashMaterial;

    private float currentHealth;

    private Stack<Material> previousMaterial;

    [HideInInspector]
    public bool invincible = true;

    private float hitFlashDuration = .05f;

    private float deathFlingStrength = 10f;
    private float deathSpinStrength = 20f;
    private float deathDuration = 1f;

    protected bool isAlive = true;

    public void Start() {
        currentHealth = MaxHealth;
        previousMaterial = new Stack<Material>();
    }

    public IEnumerator HitFlash() {
        previousMaterial.Push(renderer.material);
        renderer.material = HitFlashMaterial;
        yield return new WaitForSeconds(hitFlashDuration);
        renderer.material = previousMaterial.Pop();
    }
    public void TakeDamage(Bullet bullet) {
        if(!invincible) {
            currentHealth = Mathf.Max(0, currentHealth - bullet.damage);
            if(currentHealth == 0) {
                StartCoroutine(DeathRattle(bullet.gameObject));
            } else {
                StartCoroutine(HitFlash());
            }
        }
        Destroy(bullet.gameObject);
    }

    public IEnumerator DeathRattle(GameObject bullet) {
        isAlive = false;
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
  }