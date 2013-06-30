using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class Letter : MonoBehaviour {

    public float MaxHealth = 100;
    public Material HitFlashMaterial;

    public AudioClip HitSound;
    public AudioClip NoEffectSound;

    private float currentHealth;

    private Stack<Material> previousMaterial;

    [HideInInspector]
    public bool invincible = true;

    private float hitFlashDuration = .05f;

    private float deathFlingStrength = 10f;
    private float deathSpinStrength = 20f;
    private float deathDuration = 1f;

    public bool isAlive = true;

    public virtual void Start() {
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
                HandleDeath(bullet);
            } else {
                StartCoroutine(HitFlash());
            }
            AudioSource.PlayClipAtPoint(HitSound,
                Camera.main.transform.position);
        } else {
            AudioSource.PlayClipAtPoint(NoEffectSound,
                Camera.main.transform.position);
        }
        Destroy(bullet.gameObject);
    }

    protected virtual void HandleDeath(Bullet bullet) {
        StartCoroutine(DeathRattle(bullet.gameObject));
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