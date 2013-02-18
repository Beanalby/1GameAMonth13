using UnityEngine;
using System.Collections;

public class WeaponRanged : WeaponBase {

    public GameObject projectile;
    public float damageRadius = 2;

	new void Start () {
        base.Start();
        damage = 30;
        range = 8;
        cooldown = 2;
	}
	new void Update () {
        base.Update();
        FireWeapon();
	}

    public override void FireWeapon() {
        if(!IsInRange)
            return;
        if(IsOnCooldown)
            return;
        lastFired = Time.time;
        Projectile tmp = (Instantiate(projectile) as GameObject).GetComponent<Projectile>();
        tmp.transform.position = transform.position;
        tmp.target = target;
        tmp.launcher = this;
    }

    public void ProjectileHit(Projectile projectile) {
        // hit everything in the target radius
        Collider[] objs = Physics.OverlapSphere(projectile.transform.position, damageRadius, targetMask);
        foreach(Collider obj in objs) {
            obj.SendMessage("GotHit", this);
        }
        Destroy(projectile.gameObject);
    }
}

