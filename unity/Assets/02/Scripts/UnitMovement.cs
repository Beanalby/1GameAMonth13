using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ClusterManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
public class UnitMovement : MonoBehaviour {

    public bool isActive = true;
    public GameObject target;

    //private float attackRange = 5f;
    private float moveSpeed = 1.5f;
    private float turnSpeed = 1f;

    //private float clusterInfluence = .25f;
    private ClusterManager cm;

	// Use this for initialization
	void Start () {
        cm = GetComponent<ClusterManager>();
	}

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            isActive = false;
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 lookTarget = target.transform.position - transform.position;
        lookTarget.y = transform.position.y;
        Quaternion rotateTarget = Quaternion.LookRotation(lookTarget);
        if(isActive) {
            rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, rotateTarget, Time.deltaTime * turnSpeed));
            Vector3 fromTarget = transform.TransformPoint(Vector3.forward * moveSpeed * Time.deltaTime) - transform.position;
            Vector3 fromBias = cm.bias * Time.deltaTime;
            Vector3 toPos = transform.position + fromTarget + fromBias;
            Debug.Log(gameObject.name
                + " bias=" + fromBias.magnitude
                + ", toPos=" + fromTarget.magnitude);
            rigidbody.MovePosition(toPos);
        }
	}
}
