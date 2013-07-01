using UnityEngine;
using System.Collections;

public struct Damage {
    public int amount;
    public Transform attacker;
    public Damage(Transform attacker, int amount) {
        this.attacker = attacker;
        this.amount = amount;
    }
}

public class Ship : MonoBehaviour {

    public GameObject bulletPrefab;

    public GameObject shipHitEffect;
    public GameObject shipDeadEffect;

    public AudioClip fireSound;
    public AudioClip deadSound;

    private float maxHealth = 100;
    private float currentHealth;

    public static Ship ship = null;

    private float bulletCooldown = .1f;

    private Transform bulletLaunch;
    private float bulletSpeed = 50f;
    private float moveSpeed = 5f;
    private float turnAmount = .3f;
    private float xRange = 8f;

    private float lastFired = -1;

    public void Start() {
        if(Ship.ship != null) {
            Debug.LogError("Multiple ships found!");
            Destroy(gameObject);
        }
        Ship.ship = this;
        currentHealth = maxHealth;
        bulletLaunch = transform.FindChild("BulletLaunch");
        currentHealth = 1; // +++ DIE NAO
    }

    public void Update() {
        HandleFiring();
        HandleMovement();
    }

    public void HandleFiring() {
        if(Time.time > lastFired + bulletCooldown) {
            if(Input.GetKey(KeyCode.Space)) {
                FireBullet();
                lastFired = Time.time;
            }
        }
    }
    public void HandleMovement() {
        Vector3 pos = transform.position;
        pos.x += Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Min(xRange, Mathf.Max(-xRange, pos.x));
        transform.position = pos;
        // turn the ship slighty towards the side we're moving
        pos = new Vector3(Input.GetAxis("Horizontal") * turnAmount, 0, 1);
        transform.rotation = Quaternion.LookRotation(pos);
    }

    public void FireBullet() {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.rotation = transform.rotation;
        bullet.transform.position = bulletLaunch.transform.position;
        bullet.rigidbody.velocity = transform.forward * bulletSpeed;
        AudioSource.PlayClipAtPoint(fireSound, Camera.main.transform.position);
    }

    public void TakeDamage(Damage damage) {
        currentHealth = Mathf.Max(0, currentHealth - damage.amount);
        Debug.Log(name + " going to take " + damage.amount
                + " from " + damage.attacker
                + ", currently at " + currentHealth);
        if(currentHealth == 0) {
            DestroyShip();
        } else {
            GameObject tmp = Instantiate(shipHitEffect) as GameObject;
            tmp.transform.position = damage.attacker.position;
        }
    }

    public void DestroyShip() {
        AudioSource.PlayClipAtPoint(deadSound, Camera.main.transform.position);
        GameObject tmp = Instantiate(shipDeadEffect) as GameObject;
        tmp.transform.position = transform.position;
        GameObject.Find("WaveDriver").SendMessage("StopRunning");
        Destroy(gameObject);
    }
}
