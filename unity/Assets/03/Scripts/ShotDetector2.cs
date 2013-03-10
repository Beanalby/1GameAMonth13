using UnityEngine;
using System.Collections;

public class ShotDetector2 : MonoBehaviour {

    Vector3 reqVelocity;
    Plane reqPlane;

    public ShotDetector1 detector1;

    void Start () {
        reqVelocity = -transform.up;
        reqPlane = new Plane(reqVelocity, Vector3.zero);
    }
    public void OnTriggerEnter(Collider col) {
        if(detector1 == null) {
            return;
        }
        Vector3 proj = Vector3.Project(col.rigidbody.velocity, reqVelocity);
        if(reqPlane.GetSide(proj)) {
            // only accept it if it also just passed properly through
            // detector1.  Protects against very fast balls moving in
            // horizontally at a slightly negative angle.
            if(detector1.passedObjects.Contains(col.gameObject)) {
                Debug.Log(col.gameObject + " success!");
                detector1.passedObjects.Remove(col.gameObject);
            }
        }
    }
}
