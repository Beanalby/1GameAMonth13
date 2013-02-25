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
        if (target == null)
            return null;
        if(!IsInRange)
            return null;
        if(IsOnCooldown)
            return null;
        lastFired = Time.time;
        if(target!=null)
            target.SendMessageUpwards("GotHit", this);
        if(effectTemplate!=null) {
            WeaponEffectBase tmp = (Instantiate(effectTemplate) as GameObject).GetComponent<WeaponEffectBase>();
            tmp.launcher = weaponMuzzle;
            tmp.target = target;
        }
        return null;
    }
}
