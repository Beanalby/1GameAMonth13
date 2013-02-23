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
        if(autoFire)
            FireWeapon();
	}

    public override Projectile FireWeapon() {
        if(!IsInRange)
            return null;
        if(IsOnCooldown)
            return null;
        lastFired = Time.time;
        Projectile tmp = (Instantiate(projectile) as GameObject).GetComponent<Projectile>();
        tmp.transform.position = transform.position;
        tmp.target = target;
        tmp.launcher = this;
        return tmp;
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

