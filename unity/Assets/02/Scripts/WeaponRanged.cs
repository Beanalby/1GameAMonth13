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
            HandleFiring();
	}

    public override Projectile FireWeapon() {
        lastFired = Time.time;
        Projectile tmp = (Instantiate(projectile) as GameObject).GetComponent<Projectile>();
        tmp.transform.position = transform.position;
        tmp.target = target;
        tmp.launcher = this;
        SendMessage("Firing");
        return tmp;
    }
    private void HandleFiring() {
        if(!isActive)
            return;
        if(!IsInRange)
            return;
        if(IsOnCooldown)
            return;
        FireWeapon();
    }
    public void ProjectileHit(Projectile projectile) {
        // hit everything in the target radius
        Collider[] objs = Physics.OverlapSphere(projectile.transform.position, damageRadius, targetMask);
        foreach(Collider obj in objs) {
            obj.SendMessageUpwards("GotHit", this);
        }
        Destroy(projectile.gameObject);
    }
}

