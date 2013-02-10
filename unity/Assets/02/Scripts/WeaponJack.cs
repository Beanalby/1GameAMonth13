using UnityEngine;
using System.Collections;

public class WeaponJack : WeaponBase {

	new void Start () {
        base.Start();
        damage = 30;
        range = 3;
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
        target.SendMessage("GotHit", this);
        Debug.Log("PEW!");
    }
}

