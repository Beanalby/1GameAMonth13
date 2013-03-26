using UnityEngine;
using System.Collections;

public class Rim : MonoBehaviour {

    public float minSoundVelocity =5f;
    public int value = 2;

    Transform shadow;
    float shadowDistance;
    Vector3 shadowPosition;
    void Start () {
        shadow = transform.FindChild("RimShadow");
        shadowDistance = (transform.position - shadow.position).magnitude;
    }
    
    // Update is called once per frame
    void Update () {
        shadowPosition = transform.position;
        shadowPosition.y += shadowDistance;
        shadow.position = shadowPosition;
        shadow.rotation = Quaternion.LookRotation(Vector3.down);
    }
}
