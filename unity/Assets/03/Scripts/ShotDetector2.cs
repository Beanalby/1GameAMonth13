using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ShotDetector2 : MonoBehaviour {

    Rim rim;
    Vector3 reqVelocity;
    Plane reqPlane;

    public ShotDetector1 detector1;

    private AudioSource source;

    void Start () {
        rim = transform.parent.GetComponent<Rim>();
        if(rim == null) {
            Debug.LogError("!!! " + name + " can't find its rim!");
        }
        reqVelocity = -transform.up;
        reqPlane = new Plane(reqVelocity, Vector3.zero);
        source = GetComponent<AudioSource>();
    }
    public void OnTriggerEnter(Collider col) {
        if(detector1 == null) {
            return;
        }
        Vector3 proj = Vector3.Project(col.GetComponent<Rigidbody>().velocity, reqVelocity);
        if(reqPlane.GetSide(proj)) {
            // only accept it if it also just passed properly through
            // detector1.  Protects against very fast balls moving in
            // horizontally at a slightly negative angle.
            if(detector1.passedObjects.Contains(col.gameObject)) {
                detector1.passedObjects.Remove(col.gameObject);
                rim.SendMessage("ShotSuccess");
                if(col.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= rim.minSoundVelocity) {
                    source.Play();
                }
            }
        }
    }
}
