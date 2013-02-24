using UnityEngine;
using System.Collections;

public class ShootableThing : MonoBehaviour {

    public GameObject progressTemplate;
    public GameObject fragmentTemplate;
    public int MaxHealth = -1;
    public Vector3 offset;
    public float healthbarScale = 1f;

    private ProgressCircle pc = null;
    private int health = -1;

    public int Health {
        get { return health; }
        set { UpdateHealth(value); }
    }
	void Start () {
        health = MaxHealth;
	}
    void Update() {
        //Health = MaxHealth - (int)(20 * Time.time);
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

    public virtual void Die() {
        if (fragmentTemplate != null) {
            int num = MaxHealth / 20;
            num += Random.Range(-num / 2, num / 2);
            for (int i=0; i < num; i++) {
                GameObject tmp = Instantiate(fragmentTemplate) as GameObject;
                tmp.transform.position = transform.position;
            }
        }
        Destroy(gameObject);
    }
}

