using UnityEngine;
using System.Collections;

public class WeaponDirect : WeaponBase {

	new void Start () {
        base.Start();
        damage = 10;
        range = 3;
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
        target.SendMessage("GotHit", this);
        return null;
    }
}

