using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class Letter : MonoBehaviour {

    public float MaxHealth = 100;

    public AudioClip HitSound;
    public AudioClip NoEffectSound;
    public AudioClip deathSound;

    protected int scoreValue=1;

    private float currentHealth;

    [HideInInspector]
    public bool invincible = true;

    private float deathFlingStrength = 10f;
    private float deathSpinStrength = 20f;
    private float deathDuration = 1f;

    [HideInInspector]
    public bool isAlive = true;

    public virtual void Start() {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(Damage damage) {
        if(!invincible) {
            currentHealth = Mathf.Max(0, currentHealth - damage.amount);
            if(currentHealth == 0) {
                SendMessage("HandleDeath", damage);
            }
            AudioSource.PlayClipAtPoint(HitSound,
                Camera.main.transform.position);
        } else {
            AudioSource.PlayClipAtPoint(NoEffectSound,
                Camera.main.transform.position);
        }
    }

    protected virtual void HandleDeath(Damage damage) {
        if(Ship.ship != null) {
            Ship.ship.AddScore(scoreValue);
        }
        if(deathSound != null) {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        }
        StartCoroutine(DeathRattle(damage.attacker));
    }
    public IEnumerator DeathRattle(Transform attacker) {
        isAlive = false;
        GetComponent<ParticleSystem>().Play();
         // don't cause collisions with anything else as we fling
        GetComponent<Collider>().enabled = false;
        // clear our parent Wave so we aren't "pulled forward" anymore
        transform.parent = null;
        // throw ourselves backward based on the bullet's forward
        Vector3 dir = attacker.forward;
        dir.y = Random.Range(.3f, .6f);
        dir *= deathFlingStrength;
        GetComponent<Rigidbody>().velocity = dir;
        GetComponent<Rigidbody>().angularVelocity = new Vector3(
            Random.Range(-deathSpinStrength, deathSpinStrength),
            Random.Range(-deathSpinStrength, deathSpinStrength),
            Random.Range(-deathSpinStrength, deathSpinStrength));
        GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(deathDuration);
        Destroy(gameObject);
    }


    /// <summary>
    /// Not used in live game, lets us set the health low to test transitions
    /// </summary>
    /// <param name="amount"></param>
    public void DebugSetHealth(int amount) {
        currentHealth = amount;
    }
}