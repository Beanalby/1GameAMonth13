using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

    public GameObject bulletPrefab;

    public GameObject shipHitEffect;
    public GameObject shipDeadEffect;

    private float maxHealth = 100;
    private float currentHealth;

    public static Ship ship= null;

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
    }
    public void TakeDamage(LetterKiller letter) {
        Debug.Log("Ship got hit by " + letter.name + "! OW!");

        currentHealth = Mathf.Max(0, currentHealth - letter.kaminazeDamage);
        if(currentHealth == 0) {
            Debug.Log("Player dead!");
            Destroy(gameObject);
        } else {
            Debug.Log("Player took " + letter.kaminazeDamage + ", down to " + currentHealth);
            GameObject tmp = Instantiate(shipHitEffect) as GameObject;
            tmp.transform.position = letter.transform.position;
        }
        Destroy(letter.gameObject);
    }
}
