using UnityEngine;
using System.Collections;

public enum ShootableThingType { None, Base, Jack, Ejector, Noob };

[RequireComponent(typeof(AudioSource))]
public class ShootableThing : MonoBehaviour {

    public ShootableThingType type;
    public GameObject progressTemplate;
    public GameObject fragmentTemplate;
    public int MaxHealth = -1;
    public Vector3 offset;
    public float healthbarScale = 1f;
    public AudioClip deathSound;
    public GameObject deathEffect;
    [HideInInspector]
    public bool defaultDieAction = true;

    private bool isActive = true;
    private ProgressCircle pc = null;
    private Animator anim;
    private int health = -1;

    public bool IsActive {
        get { return isActive; }
    }
    public int Health {
        get { return health; }
        set { UpdateHealth(value); }
    }
	void Start () {
        health = MaxHealth;
        anim = GetComponentInChildren<Animator>();
	}

    public void GotHit(WeaponBase attacker) {
        Health -= attacker.Damage;
    }

    void UpdateHealth(int newHealth) {
        // if we were already dead, do nothing
        if(health == 0)
            return;

        health = newHealth;
        if(health <= 0) {
            if(pc != null) {
                Destroy(pc.gameObject);
                pc = null;
            }
            health = 0;
            SendMessage("Die");
            return;
        }
        if(health >= MaxHealth) {
            health = MaxHealth;
            if(pc != null) {
                Destroy(pc.gameObject);
                pc = null;
            }
            return;
        }
        if(pc == null) {
            pc = (Instantiate(progressTemplate) as GameObject).GetComponent<ProgressCircle>();
            pc.transform.parent = transform;
            Vector3 pos = transform.position + offset;
            // put the height slightly above the offset so it doesn't collide with a ground
            // texture, and randomize slightly so overlapping units don't flicker
            pos.y += Random.Range(.05f, .15f);
            pc.transform.position = pos;
            pc.transform.rotation = Quaternion.Euler(new Vector3(90, 180, 0));
            pc.transform.localScale = new Vector3(healthbarScale, healthbarScale, healthbarScale);
        }
        pc.Percent = ((float)health / MaxHealth);
    }
    private IEnumerator DeathAnimation() {
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    public virtual void Die() {
        if(!defaultDieAction)
            return;
        isActive = false;
        if (fragmentTemplate != null) {
            int num = MaxHealth / 20;
            num += Random.Range(-num / 2, num / 2);
            for (int i=0; i < num; i++) {
                GameObject tmp = Instantiate(fragmentTemplate) as GameObject;
                tmp.transform.position = transform.position;
            }
        }
        SendMessage("IsDead", SendMessageOptions.DontRequireReceiver);
        if(deathSound != null) {
            // playoneshot stops when gameobject's destroyed,
            // need to use the callback to keep the object around
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
            //audio.PlayOneShot(deathSound);
        }
        if(deathEffect) {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }
        if(anim) {
            StartCoroutine(DeathAnimation());
        } else {
            Destroy(gameObject);
        }
    }
}

