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

    public override void FireWeapon() {
        if(!IsInRange)
            return;
        if(IsOnCooldown)
            return;
        lastFired = Time.time;
        target.SendMessage("GotHit", this);
    }
}

