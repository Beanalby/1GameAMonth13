using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

    public GameObject bulletPrefab;

    private float bulletCooldown = .1f;

    private Transform bulletLaunch;
    private float bulletSpeed = 50f;
    private float moveSpeed = 5f;
    private float turnAmount = .3f;
    private float xRange = 8f;

    public float lastFired = -1;

    public void Start() {
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
}
