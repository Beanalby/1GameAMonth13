using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ClusterManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
[RequireComponent(typeof(WeaponBase))]
public class UnitMovement : MonoBehaviour {

    public bool isActive = true;

    //private float attackRange = 5f;
    private float moveSpeed = 15f;
    private float turnSpeed = 1f;

    private ClusterManager cm;
    private WeaponBase weapon;

	void Start () {
        cm = GetComponent<ClusterManager>();
        weapon = GetComponent<WeaponBase>();
	}
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            isActive = false;
        }
    }

	void FixedUpdate () {
        if(!isActive)
            return;
        if(weapon.target == null)
            return;
        Vector3 offset = Vector3.zero;
        Vector3 lookTarget = Vector3.zero;
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
            }
            // look towards what we're shooting at, regardless of whether
            // we scooted around at all
            lookTarget = weapon.target.transform.position - transform.position;
        } else {
            // move towards the target, with a little extra movement
            // to separate ourselves from others.  Look in the direction
            // we end up moving, rather than directly at the target.
            Vector3 moveDir = weapon.target.transform.position - transform.position;
            moveDir.y = 0;
            Vector3 fromTarget = moveDir.normalized * moveSpeed * Time.deltaTime;
            Vector3 fromBias = cm.bias * Time.deltaTime;
            offset = fromTarget + fromBias;
            lookTarget = offset;
        }
        lookTarget.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(lookTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        rigidbody.MovePosition(transform.position + offset);
    }
}
