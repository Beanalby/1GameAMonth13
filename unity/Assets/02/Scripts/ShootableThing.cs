using UnityEngine;
using System.Collections;

public class ShootableThing : MonoBehaviour {

    public GameObject progressTemplate;
    public int MaxHealth = -1;
    public Vector3 offset;
    public float scale = 1f;

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
        Debug.Log(name + " hit by " + attacker.gameObject.name + " for " + attacker.Damage + " dmg!");
        Health -= attacker.Damage;
    }
    void UpdateHealth(int newHealth) {
        // if we were already dead, do nothing
        if(health == 0)
            return;
        Debug.Log("Set health to " + newHealth);

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
            Debug.Log("Creating circle");
            pc = (Instantiate(progressTemplate) as GameObject).GetComponent<ProgressCircle>();
            pc.transform.parent = transform;
            Vector3 pos = transform.position + offset;
            pos.y += .1f;
            pc.transform.position = pos;
            pc.transform.rotation = Quaternion.Euler(new Vector3(90, 180, 0));
            pc.transform.localScale = new Vector3(scale, scale, scale);
        }
        Debug.Log("Updating percent");
        pc.Percent = ((float)health / MaxHealth);
    }
}
