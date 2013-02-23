using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public GameObject target;
    public WeaponRanged launcher;
    public float minhangTime;
    public GameObject reticle;

    private Vector3 velocity;
    private bool didHit = false;

	void Start () {
        if(target != null) {
            float hangTime;
            Vector3 diff = target.transform.position - transform.position;
            float dist = diff.magnitude;
            hangTime = Mathf.Max(dist / 15, minhangTime);
            Debug.Log("dist=" + dist.ToString(".0") + ", hangtime=" + hangTime.ToString(".0"));
            // now that we know how long it will be in the air, check if
            // the target's moving, and lead the shot accordingly.
            UnitMovement um = target.GetComponent<UnitMovement>();
            if(um != null) {
                diff += um.CurrentVelocity * hangTime;
            }
            diff /= hangTime;
            velocity = new Vector3(diff.x, (hangTime / 2) * -Physics.gravity.y, diff.z);
        }
	}
	
	void FixedUpdate () {
        velocity += Physics.gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        if(!didHit && transform.position.y <= 0) {
            didHit = true;
            launcher.ProjectileHit(this);
            if (reticle)
                Destroy(reticle);
        }
	}
}
