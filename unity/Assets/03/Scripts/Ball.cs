using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    Transform shadow;
    Vector3 offset;

    // keeps the shadow at its original position, pointed at us
    void Start() {
        shadow = transform.Find("BallShadow");
        offset = shadow.localPosition;
        shadow.parent = null;
    }
    void OnDestroy() {
        Destroy(shadow);
    }
    void Update () {
        shadow.position = transform.position + offset;
        shadow.rotation = Quaternion.LookRotation(transform.position - shadow.position);
    }
}
