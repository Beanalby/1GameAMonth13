using UnityEngine;
using System.Collections;

public class TrackPoint : MonoBehaviour {

    public TrackPoint next;

    public Vector3 direction {
        get { return (next.transform.position - transform.position).normalized; }
    }
    public float length {
        get { return (transform.position - next.transform.position).magnitude; }
    }

    public void OnDrawGizmos() {
        if(!next) {
            return;
        }
        Vector3 a = transform.position;
        Vector3 b = next.transform.position;
        a.y += .5f;
        b.y += .5f;
        Debug.DrawLine(a, b, Color.yellow);
    }
}
