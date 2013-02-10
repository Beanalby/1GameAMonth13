using UnityEngine;
using System.Collections;

/// <summary>
/// Makes sure similar units don't clump up too much.  Exposes a 
/// bias direction that would move this gameObject away from 
/// other gameObjects in the same layer.
/// </summary>
public class ClusterManager : MonoBehaviour {

    public Vector3 bias = Vector3.zero;

    private float biasStrength = 3f;
    private float checkCooldown = .1f;
    private float checkRange = 4f;
    private float lastCheck = -100;

	void Update () {
        UpdateBias();
	}
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRange);
        if(bias != Vector3.zero) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (5*bias));
        }
    }

    void UpdateBias() {
        if(lastCheck + checkCooldown > Time.time)
            return;
        lastCheck = Time.time;
        bias = Vector3.zero;
        Collider[] objs = Physics.OverlapSphere(transform.position, checkRange, 1 << gameObject.layer);
        int numUsed = 0;
        foreach(Collider obj in objs) {
            if(obj.gameObject == gameObject)
                continue;
            Vector3 back = transform.position - obj.transform.position;
            // closer has stronger bias; scale dir based on how close they are
            back = back.normalized * (checkRange - back.magnitude) / checkRange;
            bias += back;
            numUsed++;
        }
        bias *= biasStrength;
        if(numUsed > 0) {
            bias /= numUsed;
        }
    }
}
