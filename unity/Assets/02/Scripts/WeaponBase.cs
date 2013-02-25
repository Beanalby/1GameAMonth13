using System.Linq;
using System.Collections;
using UnityEngine;
public abstract class WeaponBase : MonoBehaviour {

    public string TargetLayer;
    public GameObject effectTemplate;

    protected float cooldown=1f;
    protected int damage;
    protected float lastFired=-100;
    protected float range=-1;
    //[HideInInspector]
    public GameObject target;
    protected int targetMask;
    protected bool autoTarget = true;
    protected bool autoFire = true;

    protected float retargetCooldown = .5f;
    protected float lastRetarget = -100f;
    protected GameObject weaponMuzzle;

    public abstract Projectile FireWeapon();

    public int Damage {
        get { return damage; }
    }
    public virtual bool IsInRange {
        get { return target == null || range > (target.transform.position - transform.position).magnitude; }
    }
    public virtual bool IsOnCooldown {
        get { return (lastFired + cooldown) > Time.time; }
    }

    public void Start() {
        targetMask = 1 << LayerMask.NameToLayer(TargetLayer);
        //weaponMuzzle = transform.FindChild("WeaponMuzzle").gameObject;
        Debug.Log(name + " finding muzzle");
        weaponMuzzle = FindChild(transform, "WeaponMuzzle").gameObject;
        Debug.Log("muzzle=" + weaponMuzzle);

    }
    public void OnDrawGizmos() {
        if(target != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
    public void OnDrawGizmosSelected() {
        if(range != -1) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
    public virtual void Update() {
        if(autoTarget) {
            /// try to find a new target if we don't have one (just spawned
            /// or existing target died). Also look for a new one occasionally
            /// if we're not in range or if we're targetting a base
            if(target == null) {
                FindNewTarget();
            } else {
                if (lastRetarget + retargetCooldown < Time.time
                        && (!IsInRange || target.CompareTag("Base"))) {
                    FindNewTarget();
                }
            }
        }
    }

    protected void FindNewTarget() {
        // find the closest thing we're looking for.  
        // bases are targetable, but lower priority than all else.
        lastRetarget = Time.time;
        bool targetIsBase = false;
        target = null;
        float current = Mathf.Infinity;
        foreach(Collider col in Physics.OverlapSphere(transform.position, Mathf.Infinity, targetMask)) {
            bool colIsBase = col.gameObject.CompareTag("Base");
            bool replace = false;
            float thisDist = -1;
            if(target == null) {
                replace = true;
            } else {
                // skip bases if we already have a non-base target
                if(colIsBase && !targetIsBase) {
                    continue;
                }
                // always take a new non-base over a base
                if(targetIsBase && !colIsBase) {
                    replace = true;
                } else {
                    // take the new one if it's closer
                    Vector3 dist = col.gameObject.transform.position - transform.position;
                    thisDist = dist.sqrMagnitude;
                    if(thisDist < current) {
                        replace = true;
                    }
                }
            }
            if(replace) {
                if(thisDist == -1) {
                    Vector3 dist = col.gameObject.transform.position - transform.position;
                    thisDist = dist.sqrMagnitude;
                }
                current = thisDist;
                target = col.gameObject;
                targetIsBase = target.gameObject.CompareTag("Base");
            }
        }
    }

    public static Transform FindChild(Transform node, string name) {
        if(node.name == name) {
            return node;
        }
        for(int i = 0; i < node.GetChildCount(); i++) {
            Transform result = FindChild(node.GetChild(i), name);
            if(result != null) {
                return result;
            }
        }
        return null;
    }
}
