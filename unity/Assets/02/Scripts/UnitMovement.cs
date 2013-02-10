using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ClusterManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
[RequireComponent(typeof(WeaponBase))]
public class UnitMovement : MonoBehaviour {

    public bool isActive = true;

    //private float attackRange = 5f;
    private float moveSpeed = 1.5f;
    private float turnSpeed = 5f;

    private ClusterManager cm;
    private WeaponBase weapon;

	// Use this for initialization
	void Start () {
        cm = GetComponent<ClusterManager>();
        weapon = GetComponent<WeaponBase>();
	}

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            isActive = false;
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        if(!isActive)
            return;
        Vector3 offset = Vector3.zero;
        if(weapon.IsInRange) {
            // don't need to get closer, just slide left or right
            // so others can get closer if needed
            if(cm.bias != Vector3.zero) {
                Vector3 localBias = transform.InverseTransformPoint(transform.position + (cm.bias * Time.deltaTime));
                // move the bias magnitude in the X direction.
                // if there's someone behind us, we'll have big forward
                // with a tiny x, and we want to scoot to the side quick.
                float amt = localBias.magnitude;
                localBias.y = 0; localBias.z = 0;
                localBias.x = (localBias.x > 0 ? amt : -amt);
                offset = transform.TransformPoint(localBias) - transform.position;
                rigidbody.MovePosition(transform.position + offset);
            }
            // look towards what we're shooting at, regardless of whether
            // we scooted around at all
            Quaternion targetRot = Quaternion.LookRotation(weapon.target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        } else {
            // move towards the target, with a little extra movement
            // to separate ourselves from others.  Look in the direction
            // we end up moving, rather than directly at the target.
            Vector3 moveDir = weapon.target.transform.position - transform.position;
            Vector3 fromTarget = moveDir.normalized * moveSpeed * Time.deltaTime;
            Vector3 fromBias = cm.bias * Time.deltaTime;
            offset = fromTarget + fromBias;
            transform.rotation = Quaternion.LookRotation(offset);
            rigidbody.MovePosition(transform.position + offset);
        }
    }
}
