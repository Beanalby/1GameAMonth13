using System.Collections;
using UnityEngine;
public abstract class WeaponBase : MonoBehaviour {

    protected float cooldown;
    protected float lastFired=-100;
    protected float range=-1;
    public GameObject target;

    public abstract void FireWeapon();

    public void Start() {
    }
    public void OnDrawGizmos() {
        if(range != -1) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }

    public virtual bool IsInRange {
        get { return range > (target.transform.position - transform.position).magnitude; }
    }
    public virtual bool IsOnCooldown {
        get { return (lastFired + cooldown) > Time.time; }
    }
}
