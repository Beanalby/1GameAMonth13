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
            HandleFiring();
	}

    public override Projectile FireWeapon() {
        lastFired = Time.time;
        if(target!=null)
            target.SendMessageUpwards("GotHit", this);
        if(effectTemplate!=null) {
            WeaponEffectBase tmp = (Instantiate(effectTemplate) as GameObject).GetComponent<WeaponEffectBase>();
            tmp.launcher = weaponMuzzle;
            tmp.target = target;
        }
        SendMessage("Firing");
        return null;
    }
    private void HandleFiring() {
        if(!isActive)
            return;
        if (target == null)
            return;
        if(!IsInRange)
            return;
        if(IsOnCooldown)
            return;
        FireWeapon();
    }
}
