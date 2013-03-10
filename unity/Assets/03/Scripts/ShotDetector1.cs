using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShotDetector1 : MonoBehaviour {

    Vector3 reqVelocity;
    Plane reqPlane;

    [HideInInspector]
    public List<GameObject> passedObjects;

    // Use this for initialization
    void Start () {
        passedObjects = new List<GameObject>();
        reqVelocity = -transform.up;
        reqPlane = new Plane(reqVelocity, Vector3.zero);
    }
    
    public void OnTriggerEnter(Collider col) {
        Vector3 proj = Vector3.Project(col.rigidbody.velocity, reqVelocity);
        if(reqPlane.GetSide(proj)) {
            passedObjects.Add(col.gameObject);
        }
    }
}
